using Aplicacion.Services;
using Dominio.Models;
using Dominio.Mapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Responses;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ParametroDetalleController : ControllerBase
    {
        private readonly IGenericService<ParametroDetalle> _service;
        private readonly ParametroService _parametroService;
        public ParametroDetalleController(IGenericService<ParametroDetalle> service, ParametroService parametroService)
        {
            this._service = service;
            this._parametroService = parametroService;  
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParametroDetalle>>> GetParametroDetalle()
        {
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se encontraron los parámetro detalle", Codigo = HttpStatusCode.OK };
            IEnumerable<ParametroDetalle> ActividadesModel = null;
            if (!await _service.ExistsAsync(e => e.Id > 0))
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existen parámetro detalle", Codigo = HttpStatusCode.Accepted };
            }
            else
            {
                ActividadesModel = await _service.GetAsync();
            }
            var listModelResponse = new ListModelResponse<ParametroDetalle>(response.Codigo, response.Titulo, response.Mensaje, ActividadesModel);
            return StatusCode((int)listModelResponse.Codigo, listModelResponse);
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<IEnumerable<ParametroDetalleMapper>>> porParametroId(long Id)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };

            var ParametroDetalle = _parametroService.getParametroPorId(Id);

            if (ParametroDetalle.Count() == 0)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetros detalle para asociados al Id " + Id, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvieron  parámetros detalle con el Id solicitado", Codigo = HttpStatusCode.OK };
            }

            var modelResponse = new ListModelResponse<ParametroDetalleMapper>(response.Codigo, response.Titulo, response.Mensaje, ParametroDetalle);
            return StatusCode((int)modelResponse.Codigo, modelResponse);
        }

        [HttpGet("{CodigoInterno}/{CodigoInternoDetalle}")]
        public ActionResult<IEnumerable<ParametroDetalleMapper>> porCodigoInternoDetalle(string CodigoInterno, string CodigoInternoDetalle)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };

            var ParametroDetalle = _parametroService.getParametroDetallePorCodigoInterno(CodigoInterno, CodigoInternoDetalle);

            if (ParametroDetalle.Count() == 0)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetros detalle asociados al código interno " + CodigoInterno + "código interno detalle "+ CodigoInternoDetalle,  Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvieron parámetros detalle con el Id solicitado", Codigo = HttpStatusCode.OK };
            }

            var modelResponse = new ListModelResponse<ParametroDetalleMapper>(response.Codigo, response.Titulo, response.Mensaje, ParametroDetalle);
            return StatusCode((int)modelResponse.Codigo, modelResponse);

        }


        [HttpGet("{CodigoInterno}")]
        public  ActionResult<IEnumerable<ParametroDetalleMapper>> porCodigoInterno(string CodigoInterno)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };
            
            var ParametroDetalle = _parametroService.getParametroPorCodigoInterno(CodigoInterno);

            if (ParametroDetalle.Count() == 0)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetros detalle asociados al código interno " + CodigoInterno, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvieron parámetros detalle con el Id solicitado", Codigo = HttpStatusCode.OK };
            }

            var modelResponse = new ListModelResponse<ParametroDetalleMapper>(response.Codigo, response.Titulo, response.Mensaje, ParametroDetalle);
            return StatusCode((int)modelResponse.Codigo, modelResponse);

        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ParametroDetalle>> GetParametroDetalleId(long Id)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };
            ParametroDetalle ActividadModel = null;
            if (!await _service.ExistsAsync(e => e.Id > 0))
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existen parámetro detalle", Codigo = HttpStatusCode.BadRequest };
            }

            var Actividad = await _service.GetAsync(e => e.Id == Id, e => e.OrderBy(e => e.Id), "");

            if (Actividad.Count < 1)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetro detalle con id " + Id, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                ActividadModel = Actividad.First();
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvo parámetro detalle con el Id solicitado", Codigo = HttpStatusCode.OK };
            }

            var modelResponse = new ModelResponse<ParametroDetalle>(response.Codigo, response.Titulo, response.Mensaje, ActividadModel);
            return StatusCode((int)modelResponse.Codigo, modelResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ParametroDetalle>> PostParametroDetalle(ParametroDetalle parametroDetalle)
        {
            var response = new { Titulo = "Bien Hecho!", Mensaje = "ParametroDetalle creado de forma correcta", Codigo = HttpStatusCode.Created };
            ParametroDetalle ActividadModel = null;

            bool guardo = await _service.CreateAsync(parametroDetalle);
            if (!guardo)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No se pudo guardar parámetro detalle", Codigo = HttpStatusCode.BadRequest };
            }
            else
            {
                ActividadModel = parametroDetalle;
            }

            var modelResponse = new ModelResponse<ParametroDetalle>(response.Codigo, response.Titulo, response.Mensaje, ActividadModel);
            return StatusCode((int)modelResponse.Codigo, modelResponse);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutParametroDetalle(long Id, ParametroDetalle parametroDetalle)
        {
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se actualizó parámetro detalle de forma correcta", Codigo = HttpStatusCode.OK };

            if (Id != parametroDetalle.Id)
            {
                response = new { Titulo = "Algo salió mal!", Mensaje = "El id de parámetro detalle no corresponde con el del modelo", Codigo = HttpStatusCode.BadRequest };
            }
            else if (parametroDetalle.Id < 1)
            {
                response = new { Titulo = "Algo salió mal!", Mensaje = "El modelo de parámetro detalle no tiene el campo Id ", Codigo = HttpStatusCode.BadRequest };
            }
            else
            {
                var Actividad = await _service.FindAsync(Id);

                if (Actividad == null)
                {
                    response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetro detalle con id " + Id, Codigo = HttpStatusCode.NotFound };
                }
                else
                {
                    bool updated = await _service.UpdateAsync(Id, parametroDetalle);

                    if (!updated)
                    {
                        response = new { Titulo = "Algo salió mal!", Mensaje = "No fue posible actualizar parámetro detalle", Codigo = HttpStatusCode.NoContent };
                    }
                }
            }
            var updateResponse = new GenericResponse(response.Codigo, response.Titulo, response.Mensaje);
            return StatusCode((int)updateResponse.Codigo, updateResponse);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteParametroDetalle(long Id)
        {
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se eliminó parámero detalle de forma correcta", Codigo = HttpStatusCode.OK };
            var Actividad = await _service.FindAsync(Id);

            if (Actividad == null)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámero detalle con id " + Id, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                bool elimino = await _service.DeleteAsync(Id);
                if (!elimino)
                {
                    response = new { Titulo = "Algo salió mal!", Mensaje = "No se pudo eliminar parámero detalle con Id " + Id, Codigo = HttpStatusCode.NoContent };
                }
            }
            var updateResponse = new GenericResponse(response.Codigo, response.Titulo, response.Mensaje);
            return StatusCode((int)updateResponse.Codigo, updateResponse);
        }

        [HttpGet("{CodigoInterno}/{IdDetalle}")]
        public ActionResult<IEnumerable<ParametroDetalleMapper>> porCodigoInternoDetallePadre(string CodigoInterno, long IdDetalle)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };

            var ParametroDetalle = _parametroService.getParametroDetallePorCodigoInternoPadre(CodigoInterno, IdDetalle);

            if (ParametroDetalle.Count() == 0)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetros detalle asociados al código interno " + CodigoInterno + " y Id detalle " + IdDetalle, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvieron parámetros detalle con el Id solicitado", Codigo = HttpStatusCode.OK };
            }

            var modelResponse = new ListModelResponse<ParametroDetalleMapper>(response.Codigo, response.Titulo, response.Mensaje, ParametroDetalle);
            return StatusCode((int)modelResponse.Codigo, modelResponse);

        }

        [HttpGet("{IdDetalle}")]
        public ActionResult<IEnumerable<ParametroDetalleMapper>> getParametroDetallePorPadre(long IdDetalle)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };

            var ParametroDetalle = _parametroService.getParametroDetallePorPadre(IdDetalle);

            if (ParametroDetalle.Count() == 0)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetros detalle asociados al código interno  y Id detalle " + IdDetalle, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvieron parámetros detalle con el Id solicitado", Codigo = HttpStatusCode.OK };
            }

            var modelResponse = new ListModelResponse<ParametroDetalleMapper>(response.Codigo, response.Titulo, response.Mensaje, ParametroDetalle);
            return StatusCode((int)modelResponse.Codigo, modelResponse);

        }
    }
}
