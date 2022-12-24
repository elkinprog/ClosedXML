using Dominio.Mapper;
using Persistencia.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Services
{
    public class ParametroService
    {
        public ParametroRepository _parametroRepository { get; }
        public ParametroService( ParametroRepository parametroRepository) 
        {
            this._parametroRepository = parametroRepository;
        }

        public IEnumerable<ParametroDetalleMapper> getParametroPorCodigoInterno(string codigoIterno)
        {
            return _parametroRepository.getParametroPorCodigoInterno(codigoIterno); 
        }

        public IEnumerable<ParametroDetalleMapper> getParametroPorId(long Id)
        {
            return _parametroRepository.getParametroPorId(Id);
        }

        public IEnumerable<ParametroDetalleMapper> getParametroDetallePorCodigoInterno(string codigoIterno, string codigoInternoDetalle)
        {
            return _parametroRepository.getParametroDetallePorCodigoInterno(codigoIterno,codigoInternoDetalle);
        }

        public IEnumerable<ParametroDetalleMapper> getParametroDetallePorCodigoInternoPadre(string codigoIterno, long IdDetalle)
        {
            return _parametroRepository.getParametroDetallePorCodigoInternoPadre(codigoIterno, IdDetalle);
        }

        public IEnumerable<ParametroDetalleMapper> getParametroDetallePorPadre(long IdDetalle)
        {
            return _parametroRepository.getParametroDetallePorPadre(IdDetalle);
        }

    }
}
