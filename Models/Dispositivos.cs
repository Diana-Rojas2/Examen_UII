using System.ComponentModel.DataAnnotations;

namespace Examen_UII.Models
{
    public class Dispositivos
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Identificador { get; set; }
        [MaxLength(17)]
        public string DireccionMAC { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        [MaxLength(255)]
        public string Descripcion { get; set; }
        public int Prioridad { get; set; }
        public int UsuariosId { get; set; }
        public Usuarios Usuarios { get; set; }
        public int ZonasId { get; set; }
        public Zonas Zonas { get; set; }
        public int EstadosId { get; set; }
        public Estados Estados { get; set; }

        public ICollection<RegistrosConsumo> RegistrosConsumo { get; set; }
    }
}
