using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Examen_UII.Hubs
{
    public class NotificacionHub : Hub
    {
        public async Task EnviarNotificacion(string mensaje)
        {
            await Clients.All.SendAsync("RecibirNotificacion", mensaje);
        }

        public async Task NotificarDispositivoCerrado()
        {
            await Clients.All.SendAsync("MostrarNotificacionDispositivoCerrado");
        }

        public async Task NotificarDispositivosCerrados()
        {
            await Clients.All.SendAsync("MostrarNotificacionDispositivosCerrados");
        }

    }
}
