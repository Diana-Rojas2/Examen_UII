using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Examen_UII.Models
{
    public class Estados
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nombre { get; set; }

        public ICollection<Dispositivos> Dispositivos { get; set; }  
    }
}