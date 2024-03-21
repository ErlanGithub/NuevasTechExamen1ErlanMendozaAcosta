using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using ProductoProveedor.Contratos.Repositorio;
using ProductoProveedor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoProveedor.Implementacion
{
    public class ProductoImplementacion : IProductoRepositorio
    {
        private readonly string cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public ProductoImplementacion(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Productos";
        }

        public async Task<bool> Creates(ProductoModel productos)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(productos);
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Delete(string partitionkey, string rowkey, string etaq)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionkey, rowkey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductoModel> get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Productos' and RowKey eq '{id}'";
            await foreach (ProductoModel item in tablaCliente.QueryAsync<ProductoModel>(filter: filtro))
            {
                return item;
            }
            return null;
        }

        public async Task<List<ProductoModel>> GetAll()
        {
            List<ProductoModel> lista = new List<ProductoModel>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Productos'";

            await foreach (ProductoModel item in tablaCliente.QueryAsync<ProductoModel>(filter: filtro))
            {
                lista.Add(item);
            }
            return lista;
        }

        public async Task<bool> Update(ProductoModel productos)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(productos, productos.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
