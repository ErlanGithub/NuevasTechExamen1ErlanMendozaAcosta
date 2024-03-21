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
    public class ProveeedorImplementacion : IProveedorRepositorio
    {
        //cadena de conexion (Programa/propiedades/depurar)
        private readonly string cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public ProveeedorImplementacion(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Proveedor";
        }
        public async Task<bool> Creates(ProveedorModel proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(proveedor);
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

        public async Task<ProveedorModel> get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Proveedor' and RowKey eq '{id}'";
            await foreach (ProveedorModel item in tablaCliente.QueryAsync<ProveedorModel>(filter: filtro))
            {
                return item;
            }
            return null;
        }

        public async Task<List<ProveedorModel>> GetAll()
        {
            List<ProveedorModel> lista = new List<ProveedorModel>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'Proveedor'";

            await foreach (ProveedorModel item in tablaCliente.QueryAsync<ProveedorModel>(filter: filtro))
            {
                lista.Add(item);
            }
            return lista;
        }

        public async Task<bool> Update(ProveedorModel proveedor)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(proveedor, proveedor.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
