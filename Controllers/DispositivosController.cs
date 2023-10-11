using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
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
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Registrar()
        {
            // Obtener las listas de usuarios, zonas y estados desde la base de datos
            var listaUsuarios = _db.Usuarios.Select(u => new SelectListItem
            {
                Value = u.ID.ToString(),
                Text = u.Nombre
            }).ToList();

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

            // Crear un nuevo objeto Dispositivos
            var dispositivo = new Dispositivos();

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

                return RedirectToAction("Index", "Home");
            }

            return View(dispositivo);
        }

    }
}