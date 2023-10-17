using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Examen_UII.Hubs;
using Examen_UII.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Examen_UII.Controllers
{
    [Authorize]
    public class DispositivosController : Controller
    {
        private readonly AguaDBContext _db;
        private readonly IHubContext<MapHub> _hubContext;

        private readonly IHubContext<NotificacionHub> _notificacionHubContext;

        public DispositivosController(AguaDBContext db, IHubContext<MapHub> hubContext, IHubContext<NotificacionHub> notificacionHubContext)
        {
            _db = db;
            _hubContext = hubContext;
            _notificacionHubContext = notificacionHubContext;
        }


        public IActionResult Mapa()
        {
            int userId = Convert.ToInt32(User.FindFirstValue("userId"));

            List<Dispositivos> dispositivos;

            if (User.IsInRole("Admin"))
            {
                dispositivos = _db.Dispositivos.ToList();
            }
            else
            {
                dispositivos = _db.Dispositivos
                    .Where(d => d.UsuariosId == userId)
                    .ToList();
            }

            return View(dispositivos);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Registrar()
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

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Registrar(Dispositivos dispositivo)
        {
            if (ModelState.IsValid)
            {
                if (dispositivo.EstadosId == 1)
                {
                    _db.Dispositivos.Add(dispositivo);

                    await _db.SaveChangesAsync();
                    var registroConsumo = new RegistrosConsumo
                    {
                        DispositivosId = dispositivo.ID,
                        Inicio = DateTime.Now
                    };
                    _db.RegistrosConsumo.Add(registroConsumo);

                    await _db.SaveChangesAsync();

                }
                else if (dispositivo.EstadosId == 2)
                {
                    _db.Dispositivos.Add(dispositivo);
                    await _db.SaveChangesAsync();

                }
                else
                {
                    return RedirectToAction("Error", "Usuarios");
                }
                await _notificacionHubContext.Clients.All.SendAsync("DeviceCreate", dispositivo.ID);
                return RedirectToAction("Consultar");

            }

            return View(dispositivo);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Consultar()
        {

            var dispositivos = _db.Dispositivos
            .Include(d => d.Usuarios)
            .Include(d => d.Zonas)
            .Include(d => d.Estados)
            .ToList();

            return View(dispositivos);

        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var dispositivo = await _db.Dispositivos.FindAsync(id);
            int userId = Convert.ToInt32(User.FindFirstValue("userId"));

            if (dispositivo != null)
            {
                if (dispositivo.UsuariosId == userId || User.IsInRole("Admin"))
                {
                    dispositivo.EstadosId = 1;

                    var nuevoRegistro = new RegistrosConsumo
                    {
                        DispositivosId = dispositivo.ID,
                        Inicio = DateTime.Now
                    };

                    _db.RegistrosConsumo.Add(nuevoRegistro);

                    await _db.SaveChangesAsync();

                    await _notificacionHubContext.Clients.All.SendAsync("DeviceOpened", id);
                    return RedirectToAction("Mapa");

                }
                else
                {
                    return RedirectToAction("NoPermitido", "Usuarios");
                }
            }
            else
            {
                return RedirectToAction("Error", "Usuarios");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var dispositivo = await _db.Dispositivos.FindAsync(id);

            if (dispositivo != null)
            {
                _db.Dispositivos.Remove(dispositivo);
                await _db.SaveChangesAsync();
                await _notificacionHubContext.Clients.All.SendAsync("DeviceDelete", id);
                return RedirectToAction("Consultar");
            }
            else
            {
                return RedirectToAction("Error", "Usuarios");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GraficaEstados()
        {
            return View();
        }
        public IActionResult ObtenerDatosDispositivos()
        {
            try
            {
                var dispositivosAbiertos = _db.Dispositivos.Count(d =>
                d.EstadosId == 1);
                var dispositivosCerrados = _db.Dispositivos.Count(d =>
                d.EstadosId == 2);
                var data = new
                {
                    Abiertos = dispositivosAbiertos,
                    Cerrados = dispositivosCerrados
                };
                var jsonData = Json(data);
                return jsonData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos de dispositivos: {ex.Message} ");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GraficaDatos()
        {
            return View();
        }
        public IActionResult ObtenerDatos()
        {
            try
            {
                var dispositivos = _db.Dispositivos.Include(d => d.RegistrosConsumo).ToList();
                var data = new List<object>();

                foreach (var dispositivo in dispositivos)
                {
                    var litrosRegistrados = dispositivo.RegistrosConsumo.Sum(r => r.CantidadLitrosRegistrados);
                    int estadoId = dispositivo.EstadosId;
                    int estado = estadoId == 1 ? 1 : 2;

                    var dispositivoData = new
                    {
                        Id = dispositivo.ID,
                        Prioridad = dispositivo.Prioridad,
                        Zona = dispositivo.ZonasId,
                        LitrosRegistrados = litrosRegistrados,
                        Estado = estado
                    };
                    data.Add(dispositivoData);
                }

                return Json(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos de dispositivos: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        public IActionResult ErrorD()
        {
            return View();
        }

    }
}