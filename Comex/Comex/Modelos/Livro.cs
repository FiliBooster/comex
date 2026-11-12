using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comex.Interfaces;

namespace Comex.Modelos
{
    internal class Livro: Produto, IIdentificador
    {
        public string Isbn { get; set; }
        public int TotalDePaginas { get; set; }

        [SetsRequiredMembers]
        public Livro(string nome, string descricao, float precoUnitario, string isbn, int totalDePaginas): base(Nome: nome, Descricao: descricao, Unitario: precoUnitario)
        {
            Isbn = isbn;
            TotalDePaginas = totalDePaginas;
        }
        public string Identificar()
        {
            string identificacao = $"nome: {this.Nome} | ISBN: {this.Isbn}";
            return identificacao;
        }

    }
}
