using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProductoProveedor.Contratos.Repositorio;
using ProductoProveedor.Model;
using System.Net;
using System.Text.Json;
namespace ProductoProveedor.EndPoint
{
    public class ProveedorFuncion
    {
        private readonly ILogger<ProveedorFuncion> _logger;
        private readonly IProveedorRepositorio repos;


        public ProveedorFuncion(ILogger<ProveedorFuncion> logger, IProveedorRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        [Function("ProveedorFuncion")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

        /*----------------------Insertar--------------*/
        [Function("ProveedorInsertarFuncion")]
        public async Task<HttpResponseData> InsertarProveedor([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<ProveedorModel>() ?? throw new Exception("debe ingresar un nuevo registro con todos los datos");
                registro.RowKey = Guid.NewGuid().ToString();
                registro.Timestamp = DateTime.UtcNow;
                bool sw = await repos.Creates(registro);
                if (sw)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                    return respuesta;
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        /*----------------------Listar todos--------------*/
        [Function("ProveedorListarTodosFuncion")]
        public async Task<HttpResponseData> ListarTodosProveedor([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var lista = repos.GetAll();
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(lista.Result);
                return respuesta;
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
                return respuesta;
            }

        }
        /*----------------------Listar un registro--------------*/
        [Function("ProveedorListarUnoFuncion")]
        public async Task<HttpResponseData> ListarUnoEstudio(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerproveedor/{id}")] HttpRequestData req,
         string id)
        {
            HttpResponseData respuesta;
            var registro = await repos.get(id);

            if (registro != null)
            {
                respuesta = req.CreateResponse(HttpStatusCode.OK);
                respuesta.Headers.Add("Content-Type", "application/json");
                await respuesta.WriteStringAsync(JsonSerializer.Serialize(registro));
            }
            else
            {
                respuesta = req.CreateResponse(HttpStatusCode.NotFound);
            }

            return respuesta;
        }
        /*----------------------Editar-----------------------*/
        [Function("ProveedorEditarFuncion")]
        public async Task<HttpResponseData> EditarExperiencia(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "editarProveedor/proveedor")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            try
            {
                string requestBody = await req.ReadAsStringAsync();
                ProveedorModel experiencia = JsonSerializer.Deserialize<ProveedorModel>(requestBody);
                bool success = await repos.Update(experiencia);

                if (success)
                {
                    respuesta = req.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception)
            {
                respuesta = req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return respuesta;
        }
        /*----------------------Eliminar--------------*/
        [Function("ProveedorEliminarFuncion")]
        public async Task<HttpResponseData> EliminarExperiencia(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarproveedor/{partitionkey}/{rowkey}")] HttpRequestData req,
                                string partitionkey,
                                string rowkey)
        {
            HttpResponseData respuesta;
            string etag = "";
            bool sw = await repos.Delete(partitionkey, rowkey, etag);
            if (sw)
            {
                respuesta = req.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                respuesta = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return respuesta;
        }
    }
}
