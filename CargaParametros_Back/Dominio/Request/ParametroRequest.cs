using System.Data;

namespace Dominio.Request
{
    public class ParametroRequest
    {
        public DataTable Parametros { get; set; }

        public DataTable ParametroDetalles { get; set; }
        //public List<Parametro>? Parametros { get; set; }
        //public List<ParametroDetalle>? ParametroDetalles { get; set; } 
        public List<string>? Errores { get; set; }
        public int Registros { get; set; }

    }
}
