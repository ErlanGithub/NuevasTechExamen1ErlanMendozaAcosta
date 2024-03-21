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
    public class ProductoFunction
    {
        private readonly ILogger<ProductoFunction> _logger;
        private readonly IProductoRepositorio repos;

        public ProductoFunction(ILogger<ProductoFunction> logger, IProductoRepositorio repos)
        {
            _logger = logger;
            this.repos = repos;
        }

        /*----------------------Insertar--------------*/
        [Function("ExperienciaInsertarFuncion")]
        public async Task<HttpResponseData> InsertarExperiencia([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            HttpResponseData respuesta;
            try
            {
                var registro = await req.ReadFromJsonAsync<ProductoModel>() ?? throw new Exception("debe ingresar un nuevo registro con todos los datos");
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
        [Function("ProductosListarTodosFuncion")]
        public async Task<HttpResponseData> ListarTodosProductos([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
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
        [Function("ProductoListarUnoFuncion")]
        public async Task<HttpResponseData> ListarUnProducto(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "obtenerproducto/{id}")] HttpRequestData req,
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
        [Function("ProductoEditarFuncion")]
        public async Task<HttpResponseData> EditarProducto(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "editarproducto/producto")] HttpRequestData req)
        {
            HttpResponseData respuesta;

            try
            {
                string requestBody = await req.ReadAsStringAsync();
                ProductoModel productos = JsonSerializer.Deserialize<ProductoModel>(requestBody);
                bool success = await repos.Update(productos);

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
        [Function("ProductosEliminarFuncion")]
        public async Task<HttpResponseData> EliminarProductos(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarProductos/{partitionkey}/{rowkey}")] HttpRequestData req,
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
