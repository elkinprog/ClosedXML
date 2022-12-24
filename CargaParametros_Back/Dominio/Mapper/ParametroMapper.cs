namespace Dominio.Mapper
{
    public class ParametroMapper
    {
        public long Id { get; set; }

        public String? VcNombre { get; set; }

        public String? VcCodigoInterno { get; set; }

        public Boolean BEstado { get; set; }

        public DateTime DtFechaCreacion { get; set; }

        public DateTime? DtFechaActualizacion { get; set; }

        public DateTime? DtFechaAnulacion { get; set; }
        
        public IEnumerable<ParametroDetalleMapper> ParametroDetalles { get; set; }
            = Array.Empty<ParametroDetalleMapper>();

    }
}
