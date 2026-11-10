using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Comex
{
    class Cliente()
    {
        public required string Nome{ get; set; }
        public required int CPF {get;set;}
        public required string Email {get;set;}
        public required string Profissao {get;set;}
        public required int Telefone {get;set;}
        public required string Endereco { get; set; }
    }
}
