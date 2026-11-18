using System;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class ExibirCarrinhoMenu : Menu
    {
        public ExibirCarrinhoMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Exibir Carrinho";

        public override void Executar()
        {
            Console.WriteLine("\nDigite o CPF do cliente cujo carrinho deseja exibir:");
            if (!CpfValidation.TryNormalize(EntradaConsole.ReadLine(), out string cpfDoCliente))
            {
                Console.WriteLine("CPF inválido.");
                return;
            }
            if (!State.CarrinhosEmAberto.TryGetValue(cpfDoCliente, out var carrinho))
            {
                Console.WriteLine("Nenhum carrinho encontrado para esse cliente.");
                return;
            }
            if (carrinho.Count == 0)
            {
                Console.WriteLine("Carrinho vazio.");
                return;
            }
            foreach (Produto produto in carrinho.Values)
            {
                decimal subtotal = produto.Quantidade * produto.Unitario;
                Console.WriteLine($"Produto: {produto.Nome} | Quantidade: {produto.Quantidade} | Preço Unitário (R$): {produto.Unitario} | Sub-total (R$): {subtotal} ");
            }
        }
    }
}
