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

                    return RedirectToAction("Consultar");
                }
                else if (dispositivo.EstadosId == 2)
                {
                    _db.Dispositivos.Add(dispositivo);
                    await _db.SaveChangesAsync();

                    return RedirectToAction("Consultar");
                }
                else
                {
                    return RedirectToAction("Error", "Usuarios");
                }

            }

            return View(dispositivo);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Consultar()
        {
            /* int userId = Convert.ToInt32(User.FindFirstValue("userId"));
            if (User.IsInRole("Admin"))
            { */
            var dispositivos = _db.Dispositivos
            .Include(d => d.Usuarios)
            .Include(d => d.Zonas)
            .Include(d => d.Estados)
            .ToList();

            return View(dispositivos);
            /* }
            else
            {
                var dispositivos = _db.Dispositivos
                .Include(d => d.Usuarios)
                .Include(d => d.Zonas)
                .Include(d => d.Estados)
                .Where(d => d.UsuariosId == userId)
                .ToList();

                return View(dispositivos);
            } */

        }

        /* [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var dispositivo = await _db.Dispositivos.FindAsync(id);

            int userId = Convert.ToInt32(User.FindFirstValue("userId"));

            if (dispositivo != null)
            {
                 if (dispositivo.UsuariosId == userId || User.IsInRole("Admin")) {
                    dispositivo.EstadosId = 1;

                    var nuevoRegistro = new RegistrosConsumo
                    {
                        DispositivosId = dispositivo.ID,
                        Inicio = DateTime.Now
                    };

                    _db.RegistrosConsumo.Add(nuevoRegistro);

                    await _db.SaveChangesAsync();

                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Mapa");
                    }
                    else
                    {
                        return RedirectToAction("Mapa");
                    }

                }else {
                    return RedirectToAction("NoPermitido", "Usuarios");
                }
            }
            else
            {
                return RedirectToAction("Error", "Usuarios");
            }
        } */


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

                    var dispositivos = _db.Dispositivos.ToList();

                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Consultar");
                    }
                    else
                    {
                        return RedirectToAction("Mapa");
                    }

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

                return RedirectToAction("Consultar");
            }
            else
            {
                return RedirectToAction("Error", "Usuarios");
            }
        }

       
    }
}