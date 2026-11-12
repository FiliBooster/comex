using System;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class CadastrarProdutoMenu : Menu
    {
        public CadastrarProdutoMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Cadastrar Produto";

        public override void Executar()
        {
            Console.WriteLine("\nDigite o nome do produto:");
            string nomeDoProduto = Console.ReadLine();

            Console.WriteLine("\nDigite o preço (em R$) do produto:");
            if (!float.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out float precoDoProduto))
            {
                Console.WriteLine("Preço inválido.");
                return;
            }

            Console.WriteLine("\nDigite a descrição do produto:");
            string descricaoDoProduto = Console.ReadLine();

            Produto novoProduto = new Produto(nomeDoProduto, descricaoDoProduto, precoDoProduto);

            State.CadastroDeProdutos[nomeDoProduto] = novoProduto;

            Scripts.TimeStampWriteLine($"Produto {nomeDoProduto} cadastrado com sucesso.");

        }
    }
}
