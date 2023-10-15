using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            ViewData["DispositivosId"] = dispositivoId; // Establece el valor del ID del dispositivo
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistrosConsumo registros)
        {
            if (ModelState.IsValid)
            {
                registros.Fin = DateTime.Now;

                // Modifica la consulta para obtener el último registro por fecha
                var registroExistente = await _db.RegistrosConsumo
                    .Where(r => r.DispositivosId == registros.DispositivosId)
                    .OrderByDescending(r => r.Inicio) // Ordenar por fecha en orden descendente
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

                var dispositivo = await _db.Dispositivos.FindAsync(registros.DispositivosId);
                if (dispositivo != null)
                {
                    dispositivo.EstadosId = 2;
                }

                await _db.SaveChangesAsync();

                return RedirectToAction("Consultar", "Dispositivos");
            }

            return View(registros);
        }


    }
}