using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IProveedor
    {
        public string nombreProveedor { get; set; }
        public string direccion { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
    }
}
