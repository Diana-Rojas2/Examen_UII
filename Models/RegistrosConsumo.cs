namespace Examen_UII.Models
{
    public class RegistrosConsumo
    {
        public int ID { get; set; }

        public int DispositivosId { get; set; }
        public Dispositivos Dispositivos { get; set; }

        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public double CantidadLitrosRegistrados { get; set; }
    }
}
