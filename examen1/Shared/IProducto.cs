using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IProducto
    {
        public string nombreProducto {  get; set; }
        public string categoria { get; set; }
        public float precio { get; set;}
        public int cantidad { get; set; }
    }
}
