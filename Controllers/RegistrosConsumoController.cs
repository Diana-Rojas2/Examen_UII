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

        public RegistrosConsumoController(AguaDBContext db, IHubContext<MapHub> hubContext)
        {
            _db = db;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult Registrar(int dispositivoId)
        {
            ViewData["DispositivosId"] = dispositivoId;
            return View();
        }

        [HttpPost]
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

                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Consultar", "Dispositivos");
                        }
                        else
                        {
                            return RedirectToAction("Mapa", "Dispositivos");
                        }
                    }
                    else
                    {
                        return RedirectToAction("NoPermitido", "Usuarios");
                    }
                }
            }

            return View(registros);
        }


    }
}