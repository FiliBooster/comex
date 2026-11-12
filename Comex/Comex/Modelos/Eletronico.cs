using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comex.Modelos
{
    internal class Eletronico: Produto
    {
        public int Voltagem { get; set; }
        public int Potencia { get; set; }

        [SetsRequiredMembers]
        public Eletronico(string nome, string descricao, float precoUnitario, int voltagem, int potencia) : base(Nome: nome, Descricao: descricao, Unitario: precoUnitario)
        {
            Voltagem = voltagem;
            Potencia = potencia;
        }
    }
}
