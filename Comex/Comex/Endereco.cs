using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Comex
{
    internal class Endereco
    {
        public required string Bairro {get;set;}
        public required string Cidade { get;set;}
        public required string Complemento { get;set;}
        public required string Estado { get; set; }
        public required string Rua { get;set;}
        public required int Numero {get;set;}
    }
}
