using System.Diagnostics.CodeAnalysis;


namespace Comex
{
    [method: SetsRequiredMembers]
    internal class Produto(string Nome, string Descricao, decimal Unitario)
    {
        public required string Nome { get; set; } = Nome;
        public required string Descricao { get; set; } = Descricao;
        public required decimal Unitario { get; set; } = Unitario;
        public int Quantidade { get; set; }
    }
}
