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
            Console.WriteLine("\nDigite o nº do CPF do cliente cujo carrinho deseja exibir:");
            if (!int.TryParse(Console.ReadLine(), out int cpfDoCliente))
            {
                Console.WriteLine("CPF inválido.");
                return;
            }
            foreach (Produto produto in State.CarrinhosEmAberto[cpfDoCliente].Values)
            {
                float subtotal = produto.Quantidade * produto.Unitario;
                Console.WriteLine($"Produto: {produto.Nome} | Quantidade: {produto.Quantidade} | Preço Unitário (R$): {produto.Unitario} | Sub-total (R$): {subtotal} ");
            }
        }
    }
}
