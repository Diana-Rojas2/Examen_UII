using System.Drawing;
using Microsoft.AspNetCore.SignalR;

namespace _2._5_WebSockets.Hubs
{
    public class MensajeHub : Hub
    {
        public async Task EnviarMensaje(string msj){
            string hora = DateTime.Now.ToShortTimeString();
            string fecha = DateTime.Now.ToShortDateString();
            string nuevo = $"Fecha: {fecha}, Hora: {hora}, Mensaje: {msj}";
            await Clients.All.SendAsync("EnviarMensajeTodos", nuevo);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UsuarioConectado",Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e){
            await Clients.All.SendAsync("UsuarioDesconectado", Context.ConnectionId);
            await base.OnDisconnectedAsync(e);
        }

        public async Task EnviarMensajeUsuario(string user, string msj){
            string usuarioAutenticado = Context.UserIdentifier;
            await Clients.User(user)
                        .SendAsync("EnviarMsjAutenticado", msj +
                        " Enviado de: " + usuarioAutenticado);
        }
    }
}