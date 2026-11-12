using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Comex.Interfaces;

namespace Comex
{
    class Cliente(): IIdentificador
    {
        public required string Nome{ get; set; }
        public required int CPF {get;set;}
        public required string Email {get;set;}
        public required string Profissao {get;set;}
        public required int Telefone {get;set;}
        public required string Endereco { get; set; }
        public string Identificar()
        {
            string identificacao = $"nome: {this.Nome} | CPF: {this.CPF}";
            return identificacao;
        }
    }
}
