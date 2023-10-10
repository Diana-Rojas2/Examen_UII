using System.ComponentModel.DataAnnotations;

namespace Examen_UII.Models
{
    public class Usuarios
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; }
        [MaxLength(100)]
        public string Usuario { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public int RolesId { get; set; }
        public Roles Roles { get; set; }
    }
}
