using Examen_UII.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Examen_UII.Hubs
{
    public class NotificacionHub : Hub
    {
        private readonly AguaDBContext _db;


        public NotificacionHub(AguaDBContext db)
        {
            _db = db;
        }


        public async Task NotificarApertura(int dispositivoId)
        {
            await Clients.All.SendAsync("DeviceOpened", dispositivoId);
        }

        public async Task NotificarCierre(int dispositivoId)
        {
            await Clients.All.SendAsync("DeviceClosed", dispositivoId);
        }

        public async Task NotificarCreado(int dispositivoId)
        {
            await Clients.All.SendAsync("DeviceCreate", dispositivoId);
        }

        public async Task NotificarEliminado(int dispositivoId)
        {
            await Clients.All.SendAsync("DeviceDelete", dispositivoId);
        }

        public async Task NotificacionCerrarUltimo(string mensaje)
        {
            await Clients.All.SendAsync("NotificacionTodosCerrados", mensaje);
        }

    }
}
