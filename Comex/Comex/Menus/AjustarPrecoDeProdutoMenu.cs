using System;
using System.Collections.Generic;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class AjustarPrecoDeProdutoMenu : Menu
    {
        public AjustarPrecoDeProdutoMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Ajustar Preço de Produto";
        public override void Executar()
        {
            Console.WriteLine("\nQual o nome do produto cujo preço deseja ajustar?");
            string nomeDoProduto = EntradaConsole.ReadLine();
            try
            {
                Produto produto = Scripts.PesquisarPorProdutoNoCadastro(nomeDoProduto);
                decimal precoAntigo = produto.Unitario;
                Console.WriteLine("\nDigite o novo preço (em R$) para o produto:");
                if (!decimal.TryParse(EntradaConsole.ReadLine(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out decimal novoPreco))
                {
                    Console.WriteLine("Preço inválido.");
                    return;
                }
                State.CadastroDeProdutos[nomeDoProduto].Unitario = novoPreco;
                Scripts.TimeStampWriteLine("Preço ajustado!");
                Console.WriteLine($"[{nomeDoProduto}]:\n preço antigo: {precoAntigo} | preço novo: {novoPreco}");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"Nenhum produto encontrado com nome [{nomeDoProduto}] no Cadastro de Produtos.");
            }
        }
    }
}
