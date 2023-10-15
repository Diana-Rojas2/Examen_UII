using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Examen_UII.Hubs
{

    public class MapHub : Hub
    {
        public async Task EnviarCoordenadas(double latitud, double longitud)
        {

            await Clients.All.SendAsync("NuevasCoordenadas", latitud, longitud);
        }
    }


}
