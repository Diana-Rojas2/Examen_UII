using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Examen_UII.Hubs;
using Examen_UII.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Examen_UII.Controllers
{
    [Authorize]
    public class RegistrosConsumoController : Controller
    {
        private readonly AguaDBContext _db;
        private readonly IHubContext<MapHub> _hubContext;

        private readonly IHubContext<NotificacionHub> _notificacionHubContext;

        public RegistrosConsumoController(AguaDBContext db, IHubContext<MapHub> hubContext, IHubContext<NotificacionHub> notificacionHubContext)
        {
            _db = db;
            _hubContext = hubContext;
            _notificacionHubContext = notificacionHubContext;
        }

        [HttpGet]
        public IActionResult Registrar(int dispositivoId)
        {
            ViewData["DispositivosId"] = dispositivoId;
            return View();
        }

        public async Task<IActionResult> Registrar(RegistrosConsumo registros)
        {
            if (ModelState.IsValid)
            {
                registros.Fin = DateTime.Now;
                int userId = Convert.ToInt32(User.FindFirstValue("userId"));
                var dispositivo = await _db.Dispositivos.FindAsync(registros.DispositivosId);

                if (dispositivo != null)
                {
                    if (dispositivo.UsuariosId == userId || User.IsInRole("Admin"))
                    {
                        var ultimoAbierto = _db.Dispositivos.Count(d => d.EstadosId == 1) == 1 && dispositivo.EstadosId == 1;

                        if (ultimoAbierto)
                        {
                            List<Usuarios> adminsList = _db.Usuarios.Include(x => x.Roles).Where(x => x.RolesId == 1).ToList();
                            if (adminsList.Any())
                            {
                                await _notificacionHubContext.Clients.All.SendAsync("NotificacionCerrarUltimo", "No puedes cerrar el último dispositivo abierto.");
                            }

                            return RedirectToAction("Mapa", "Dispositivos");
                        }

                        var registroExistente = await _db.RegistrosConsumo
                            .Where(r => r.DispositivosId == registros.DispositivosId)
                            .OrderByDescending(r => r.Inicio)
                            .FirstOrDefaultAsync();

                        if (registroExistente != null)
                        {
                            registroExistente.Fin = registros.Fin;
                            registroExistente.CantidadLitrosRegistrados = registros.CantidadLitrosRegistrados;
                        }
                        else
                        {
                            return RedirectToAction("Error", "Usuarios");
                        }

                        dispositivo.EstadosId = 2;

                        await _db.SaveChangesAsync();
                        await _notificacionHubContext.Clients.All.SendAsync("DeviceStateChanged", dispositivo.ID, dispositivo.EstadosId);
                        List<Usuarios> admins = _db.Usuarios.Include(x => x.Roles).Where(x => x.RolesId == 1).ToList();
                        if (admins.Any())
                        {
                            await _notificacionHubContext.Clients.All.SendAsync("DeviceClosed", registros.DispositivosId);
                        }
                        return RedirectToAction("Mapa", "Dispositivos");
                    }
                    else
                    {
                        return RedirectToAction("NoPermitido", "Usuarios");
                    }
                }
            }
            return View(registros);
        }


        public IActionResult Consultar()
        {
            int userId = Convert.ToInt32(User.FindFirstValue("userId"));
            if (User.IsInRole("Admin"))
            {
                var registros = _db.RegistrosConsumo
                    .Include(r => r.Dispositivos)
                    .ToList();
                return View(registros);
            }
            else
            {
                var registros = _db.RegistrosConsumo
                    .Include(r => r.Dispositivos)
                    .Where(r => r.Dispositivos.UsuariosId == userId)
                    .ToList();
                return View(registros);
            }
        }


        [Authorize(Roles = "Admin")]
        public IActionResult GraficaConsumo()
        {
            return View();
        }
        public IActionResult ObtenerLitros()
        {
            try
            {
                var totalLitrosRegistrados = _db.RegistrosConsumo.Sum(r =>
                r.CantidadLitrosRegistrados);
                var data = new
                {
                    TotalLitrosRegistrados = totalLitrosRegistrados
                };
                return Json(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener datos de dispositivos: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

    }
}