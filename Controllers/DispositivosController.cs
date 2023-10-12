using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Examen_UII.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Examen_UII.Controllers
{
    public class DispositivosController : Controller
    {
        private readonly AguaDBContext _db;

        public DispositivosController(AguaDBContext db)
        {
            _db = db;
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Mapa()
        {
            return View();
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Registrar(double? Latitud, double? Longitud)
        {
            var listaUsuarios = _db.Usuarios
                .Where(u => u.RolesId != 1)
                .Select(u => new SelectListItem
                {
                    Value = u.ID.ToString(),
                    Text = u.Nombre
                })
                .ToList();

            var listaZonas = _db.Zonas.Select(z => new SelectListItem
            {
                Value = z.ID.ToString(),
                Text = z.Nombre
            }).ToList();

            var listaEstados = _db.Estados.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Nombre
            }).ToList();

            ViewData["Usuarios"] = listaUsuarios;
            ViewData["Zonas"] = listaZonas;
            ViewData["Estados"] = listaEstados;

            var dispositivo = new Dispositivos
            {
                Latitud = (double)Latitud,
                Longitud = (double)Longitud
            };

            return View(dispositivo);
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Registrar(Dispositivos dispositivo)
        {
            if (ModelState.IsValid)
            {
                _db.Dispositivos.Add(dispositivo);
                await _db.SaveChangesAsync();

                return RedirectToAction("Consultar");
            }

            return View(dispositivo);
        }

        [Authorize]
        public IActionResult Consultar()
        {

            var dispositivo = _db.Dispositivos
                                .Include(d => d.Usuarios)
                                .Include(d => d.Zonas)
                                .Include(d => d.Estados)
                                .ToList();

            return View(dispositivo);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var dispositivo = await _db.Dispositivos.FindAsync(id);

            if (dispositivo == null)
            {
                return NotFound();
            }
            var estadoAbierto = await _db.Estados.FirstOrDefaultAsync(e => e.Nombre == "Abierto");
            var estadoCerrado = await _db.Estados.FirstOrDefaultAsync(e => e.Nombre == "Cerrado");

            if (estadoAbierto == null || estadoCerrado == null)
            {
                return BadRequest("Los estados 'Abierto' y 'Cerrado' no están definidos en la base de datos.");
            }
            dispositivo.EstadosId = (dispositivo.EstadosId == estadoAbierto.Id) ? estadoCerrado.Id : estadoAbierto.Id;

            try
            {
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, error = "Error al actualizar el estado del dispositivo." });
            }
        }

    }
}