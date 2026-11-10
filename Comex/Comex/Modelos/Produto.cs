using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Comex
{
    internal class Produto
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required float Unitario { get; set; }
        public int Quantidade { get; set; }

    }
}
