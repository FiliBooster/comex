using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        [SetsRequiredMembers]
        public Produto(string Nome, string Descricao, float Unitario)
        {
            this.Nome = Nome;
            this.Descricao = Descricao;
            this.Unitario = Unitario;
        }
    }
}
