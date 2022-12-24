using Aplicacion.ManejadorErrores;
using Newtonsoft.Json;
using System.Net;

namespace WebAPI.Midleware
{
    public class ExcepcionErroresMidleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExcepcionErroresMidleware> _logger;
        public ExcepcionErroresMidleware(RequestDelegate next, ILogger<ExcepcionErroresMidleware> logger)
        {
            this._next   = next;
            this._logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
           try 
           {
                await _next(context);
           }
            catch (Exception ex)
           {
                await ExcepcionAsincrono(context, ex, _logger);
           }

            async Task ExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ExcepcionErroresMidleware> logger)
            {
                object errores = null;
                switch (ex)
                {
                    case ExcepcionError me:
                        logger.LogError(ex, "manejador Error");
                        //errores = me.Errores;
                        errores = new { Codigo = (int)me.Codigo, Titulo = me.Titulo, Mensaje = me.Mensaje };
                        context.Response.StatusCode = (int)me.Codigo;
                        break;
                    case Exception e:
                        logger.LogError(ex, "Error de servidor");

                        string innerMessage = e.InnerException !=null ? e.InnerException.Message : "";

                        var error= string.IsNullOrWhiteSpace(e.Message) ? innerMessage : e.Message + " Cola Errores: " + innerMessage;
                        errores = new { Codigo = (int)HttpStatusCode.InternalServerError, Titulo = "Algo Salio Mal..", Mensaje = "Se genero el siguiente Error:" + error };
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                context.Response.ContentType = "aplication/json";
                if (errores!= null)
                {
                    var resultado= JsonConvert.SerializeObject( errores );
                    await context.Response.WriteAsync(resultado);
                }
            }
        }
    }
}
