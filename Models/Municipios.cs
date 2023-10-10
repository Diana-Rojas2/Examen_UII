using System.ComponentModel.DataAnnotations;

namespace Examen_UII.Models
{
    public class Municipios
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; }

        public ICollection<Zonas> Zonas { get; set; }   
    }
}