namespace Dominio.Models
{
    public class ParametroDetalle
    {
        public long Id { get; set; }

        public long ParametroId { get; set; }

        public String? VcNombre { get; set; }
            
        public String? TxDescripcion { get; set; }

        public long? IdPadre { get; set; }

        public String? VcCodigoInterno { get; set; }

        public Decimal? DCodigoIterno { get; set; }

        public Boolean BEstado { get; set; }

        public int? RangoDesde { get; set; }

        public int? RangoHasta { get; set; }



        public Parametro? Parametro { get; set; }

    }
}
