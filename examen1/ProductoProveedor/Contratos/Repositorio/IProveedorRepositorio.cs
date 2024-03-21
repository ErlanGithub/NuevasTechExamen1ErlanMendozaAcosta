using ProductoProveedor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoProveedor.Contratos.Repositorio
{
    public interface IProveedorRepositorio
    {
        public Task<bool> Creates(ProveedorModel proveedor);
        public Task<bool> Update(ProveedorModel proveedor);
        public Task<bool> Delete(string partitionkey, string rowkey, string etaq);
        public Task<List<ProveedorModel>> GetAll();
        public Task<ProveedorModel> get(string id);
    }
}
