using Aplicacion.AgregarExcel;
using Aplicacion.ManejadorErrores;
using Aplicacion.Services;
using Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.Responses;

namespace WebApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ParametroController : ControllerBase
    {
     
        private readonly IAgregarExcel _agregarExcel;

        private readonly IGenericService<Parametro> _service;
        private readonly IGenericService<ParametroDetalle> _serviceDetalle;
        public ParametroController(IGenericService<Parametro> service, IGenericService<ParametroDetalle> serviceDetalle, IAgregarExcel agregarExcel)
        {
            this._service = service;
            _serviceDetalle = serviceDetalle;
            this._agregarExcel = agregarExcel;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parametro>>> GetParametro()
        {
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se encontraron los parámetros", Codigo = HttpStatusCode.OK };
            IEnumerable<Parametro> ParametrosModel = null;
            if (!await _service.ExistsAsync(e => e.Id > 0))
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existen parámetros", Codigo = HttpStatusCode.Accepted };
            }
            else
            {
                ParametrosModel = await _service.GetAsync();
            }
            var listModelResponse = new ListModelResponse<Parametro>(response.Codigo, response.Titulo, response.Mensaje, ParametrosModel);
            return StatusCode((int)listModelResponse.Codigo, listModelResponse);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Parametro>> GetParametroId(long Id)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };
            Parametro ParametroModel = null;
            if (!await _service.ExistsAsync(e => e.Id > 0))
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existen parámetros", Codigo = HttpStatusCode.BadRequest };
            }

            var Parametro = await _service.GetAsync(e => e.Id == Id, e => e.OrderBy(e => e.Id), "");

            if (Parametro.Count < 1)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetro con id " + Id, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                ParametroModel = Parametro.First();
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvo el parámetro con el Id solicitado", Codigo = HttpStatusCode.OK };
            }
            var modelResponse = new ModelResponse<Parametro>(response.Codigo, response.Titulo, response.Mensaje, ParametroModel);
            return StatusCode((int)modelResponse.Codigo, modelResponse);
        }

        [HttpGet("porCodigoInterno/{CodigoInterno}")]
        public async Task<ActionResult<Parametro>> porCodigoInterno(string CodigoInterno)
        {
            var response = new { Titulo = "", Mensaje = "", Codigo = HttpStatusCode.Accepted };
            Parametro ParametroModel = null;
            if (!await _service.ExistsAsync(e => e.Id > 0))
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existen parámetros", Codigo = HttpStatusCode.BadRequest };
            }

            var Parametro = await _service.GetAsync(e => e.VcCodigoInterno == CodigoInterno, e => e.OrderBy(e => e.Id), "");

            if (Parametro.Count < 1)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetro con código interno " + CodigoInterno, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                ParametroModel = Parametro.First();
                response = new { Titulo = "Bien Hecho!", Mensaje = "Se obtuvo el parámetro con el Id solicitado", Codigo = HttpStatusCode.OK };
            }
            var modelResponse = new ModelResponse<Parametro>(response.Codigo, response.Titulo, response.Mensaje, ParametroModel);
            return StatusCode((int)modelResponse.Codigo, modelResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Postparametro(Parametro parametro)
        {
            try
            {
                var response = new { Titulo = "Bien Hecho!", Mensaje = "Parámetro creado de forma correcta", Codigo = HttpStatusCode.Created };
                Parametro ParametroModel = null;

                parametro.DtFechaCreacion = DateTime.Now;
                parametro.DtFechaActualizacion = DateTime.Now;
                parametro.DtFechaAnulacion = null; 
                bool guardo = await _service.CreateAsync(parametro);
                if (!guardo)
                {
                    response = new { Titulo = "Algo salio mal", Mensaje = "No se puedo guardar el parámetro", Codigo = HttpStatusCode.BadRequest };
                }
                else
                {
                    ParametroModel = parametro;
                }
                var modelResponse = new ModelResponse<Parametro>(response.Codigo, response.Titulo, response.Mensaje, ParametroModel);
                return StatusCode((int)modelResponse.Codigo, modelResponse);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutParametro(long Id, Parametro parametro)
        {
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se actualizó parámetro de forma correcta", Codigo = HttpStatusCode.OK };
            try
            {
                if (Id != parametro.Id)
                {
                    response = new { Titulo = "Algo salió mal!", Mensaje = "El id de parámetro no corresponde con el modelo", Codigo = HttpStatusCode.BadRequest };
                }
                else if (parametro.Id < 1)
                {
                    response = new { Titulo = "Algo salió mal!", Mensaje = "El modelo de parámetro no tiene el campo Id ", Codigo = HttpStatusCode.BadRequest };
                }
                else
                {
                    var Parametro = await _service.FindAsync(Id);

                    if (Parametro == null)
                    {
                        response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetro con id " + Id, Codigo = HttpStatusCode.NotFound };
                    }
                    else
                    {
                        parametro.DtFechaActualizacion = DateTime.Now;
                        bool updated = await _service.UpdateAsync(Id, parametro);

                        if (!updated)
                        {
                            response = new { Titulo = "Algo salió mal!", Mensaje = "No fue posible actualizar el parámetro", Codigo = HttpStatusCode.NoContent };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            var updateResponse = new GenericResponse(response.Codigo, response.Titulo, response.Mensaje);
            return StatusCode((int)updateResponse.Codigo, updateResponse);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteParametro(long Id)
        {
            var response = new { Titulo = "Bien Hecho!", Mensaje = "Se eliminó el parámetro de forma correcta", Codigo = HttpStatusCode.OK };
            var Parametro = await _service.FindAsync(Id);

            if (Parametro == null)
            {
                response = new { Titulo = "Algo salio mal", Mensaje = "No existe parámetro con id " + Id, Codigo = HttpStatusCode.NotFound };
            }
            else
            {
                bool elimino = await _service.DeleteAsync(Id);
                if (!elimino)
                {
                    response = new { Titulo = "Algo salió mal!", Mensaje = "No se pudo eliminar el parámetro con Id " + Id, Codigo = HttpStatusCode.NoContent };
                }
            }
            var updateResponse = new GenericResponse(response.Codigo, response.Titulo, response.Mensaje);
            return StatusCode((int)updateResponse.Codigo, updateResponse);
        }

        [HttpPost("cargar")]
        public async Task<ActionResult> Cargar([FromForm] List<IFormFile> files)
        {                                    
            var horaInicio = DateTime.Now;
            var statusOk = HttpStatusCode.OK;

            var response = new CargaParametroResponse(statusOk, "Bien Hecho!", "datos cargados", null, 0);
            try
            {
                var errores = new List<string>();
                var registros = 0;

                if (files.Count == 0)
                {
                    response = new CargaParametroResponse(
                        HttpStatusCode.BadRequest,
                        "Algo salio mal",
                        "No se recibio el archivo ",
                        errores,
                        registros
                    );
                }

                string rutaInicial = Environment.CurrentDirectory;
                var nombreArchivo = files[0].FileName;  //parametrossicuentanos.xlsx

                var archivoArray = nombreArchivo.Split(".");
                var extencion = archivoArray[archivoArray.Length - 1];

                if (extencion != "xlsx")
                {
                    response = new CargaParametroResponse(
                        HttpStatusCode.BadRequest,
                        "Algo salio mal",
                        "El Archivo no contiene el formato Excel ",
                        errores,
                        registros
                    );
                }
                else
                {
                   
                    DateTime now = DateTime.Now;
                    var horaNombre = now.ToString("yyyy-MM-dd-HH-mm-ss");
                    var rutaArchivo = rutaInicial + "/Upload/" + horaNombre + nombreArchivo;

                    if (files.Count == 1)
                    {
                        if (System.IO.File.Exists(rutaArchivo))
                        {
                            System.IO.File.Delete(rutaArchivo);
                        }
                    }

                    using (var str = System.IO.File.Create(rutaArchivo))
                    {
                        str.Position = 0;
                        await files[0].CopyToAsync(str);
                    }

                    var responseParametro = _agregarExcel.procesarArchivo(rutaArchivo);
                  

                    if (responseParametro.Errores.Count > 0)
                    {
                        response = new CargaParametroResponse(
                            HttpStatusCode.BadRequest,
                            "Datos Vacios en Documento Excel",
                            "Falta un valor en alguna celda del archivo excel",
                            responseParametro.Errores,
                            registros
                        );
                    }
                    else if (responseParametro.Registros == 0)
                    {
                        response = new CargaParametroResponse(
                            statusOk,
                            "Archivo sin procesar",
                            "No se procesó el archivo debido a que ya existían los parámetros en base de datos",
                            errores,
                            registros
                        );
                    }
                    else
                    {
                            response = new CargaParametroResponse(
                            statusOk,
                            "Parametros cargados", "Se cargaron (" + responseParametro.Registros + ") parametros y ParametrosDetalle, archivo: " + nombreArchivo,
                            responseParametro.Errores,
                            responseParametro.Registros
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                response = new CargaParametroResponse(
                    HttpStatusCode.BadRequest,
                    "Algo salio mal",
                    "Error procesando el archivo " + ex.Message + " " + ex.StackTrace,
                    null,
                    0
               );
            }
            var horaFin = DateTime.Now;
            var tiempo = horaFin - horaInicio;
            response.Mensaje += " Petición resulta en " + tiempo.ToString();
            return StatusCode((int)response.Codigo, response);
        }
    }
}





