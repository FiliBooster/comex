using System;
using System.Collections.Generic;
using System.Linq;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class ListarProdutosMenu : Menu
    {
        public ListarProdutosMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Lista de Produtos Cadastrados";

        public override void Executar()
        {
            Scripts.TimeStampWriteLine($"\nLista de produtos cadastrados agora:");

            IEnumerable<Produto> produtosOrdenados = OrdenarProdutos(State.CadastroDeProdutos.Values);

            int indice = 0;
            foreach (Produto produto in produtosOrdenados)
            {
                Console.WriteLine($"{indice}: Nome: {produto.Nome} | Preço (R$): {produto.Unitario}");
                indice++;
            }
        }

        private static IEnumerable<Produto> OrdenarProdutos(IEnumerable<Produto> produtos)
        {
            Console.WriteLine("\nComo deseja ordenar os produtos?");
            Console.WriteLine("1: Nome (A-Z)");
            Console.WriteLine("2: Nome (Z-A)");
            Console.WriteLine("3: Preço (menor para maior)");
            Console.WriteLine("4: Preço (maior para menor)");
            Console.WriteLine("Qualquer outra tecla: manter ordem original");

            string opcao = Console.ReadLine();

            return opcao switch
            {
                "1" => produtos.OrderBy(p => p.Nome, StringComparer.OrdinalIgnoreCase),
                "2" => produtos.OrderByDescending(p => p.Nome, StringComparer.OrdinalIgnoreCase),
                "3" => produtos.OrderBy(p => p.Unitario),
                "4" => produtos.OrderByDescending(p => p.Unitario),
                _ => produtos
            };
        }
    }
}
