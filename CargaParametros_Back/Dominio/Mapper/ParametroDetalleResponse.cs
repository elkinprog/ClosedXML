using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Mapper
{
    public class ParametroDetalleResponse<T>
    {
        public IEnumerable<T> Resultados { get; set; }
        public ParametroDetalleResponse(IEnumerable<T> resultados = null)
        {
            Resultados = resultados?? Array.Empty<T>();  
        }
    }
}
