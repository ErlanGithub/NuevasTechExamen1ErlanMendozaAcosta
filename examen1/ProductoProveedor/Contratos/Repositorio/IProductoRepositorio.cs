using ProductoProveedor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoProveedor.Contratos.Repositorio
{
    public interface IProductoRepositorio
    {
        public Task<bool> Creates(ProductoModel productos);
        public Task<bool> Update(ProductoModel productos);
        public Task<bool> Delete(string partitionkey, string rowkey, string etaq);
        public Task<List<ProductoModel>> GetAll();
        public Task<ProductoModel> get(string id);
    }
}
