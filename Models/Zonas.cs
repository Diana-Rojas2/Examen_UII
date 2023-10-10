using System.ComponentModel.DataAnnotations;

namespace Examen_UII.Models
{
    public class Zonas
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; }

        public int MunicipiosId { get; set; }
        public Municipios Municipios { get; set; }

        public ICollection<Dispositivos> Dispositivos { get; set; }

    }
}
