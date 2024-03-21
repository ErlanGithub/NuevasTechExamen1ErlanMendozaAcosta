using Azure;
using Azure.Data.Tables;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoProveedor.Model
{
    public class ProveedorModel : IProveedor, ITableEntity
    {
        public string nombreProveedor { get; set; }
        public string direccion { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
