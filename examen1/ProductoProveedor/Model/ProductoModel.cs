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
    public class ProductoModel : IProducto, ITableEntity
    {
        public string nombreProducto { get; set; }
        public string categoria { get; set; }
        public float precio { get; set; }
        public int cantidad { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
