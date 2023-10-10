using System.ComponentModel.DataAnnotations;

namespace Examen_UII.Models
{
    public class Roles
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Rol { get; set; }

        public ICollection<Usuarios> Usuarios { get; set; }
    }
}
