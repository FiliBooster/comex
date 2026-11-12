using System;
using System.Collections.Generic;
using System.Linq;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class AdicionarProdutosAoCarrinhoMenu : Menu
    {
        public AdicionarProdutosAoCarrinhoMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Adicionar Produtos Ao Carrinho";

        public override void Executar()
        {
            (int cpfDoCliente, bool clienteJaTemCadastro) = Scripts.VerificarSeClienteJaTemCadastro();
            if (clienteJaTemCadastro == false) { return; }

            Dictionary<int, Dictionary<string, Produto>> carrinho = Scripts.ObterCarrinho(cpfDoCliente);

            bool adicaoConcluida = false;
            bool produtoTemCadastro = false;
            string nomeDoProdutoDesejado = "";
            int quantidadeDesejadaDoProdutoDesejado = 0;
            while (adicaoConcluida == false)
            {
                (produtoTemCadastro, nomeDoProdutoDesejado) = Scripts.VerificarSeProdutoJaTemCadastro();
                Console.WriteLine($"-- Digite '0' para sair.");
                if (Console.ReadKey().Key is ConsoleKey.D0) { break; }

                if (produtoTemCadastro == false) { continue; }

                Console.WriteLine($"\nQual a quantidade do produto {nomeDoProdutoDesejado} deseja adicionar ao carrinho?");
                if (!int.TryParse(Console.ReadLine(), out quantidadeDesejadaDoProdutoDesejado))
                {
                    Console.WriteLine("Quantidade inválida.");
                    continue;
                }
                if (quantidadeDesejadaDoProdutoDesejado == 0) { continue; } //se quantidade é zero, não adiciona produto

                AdicionarProdutoAoCarrinho(nomeDoProduto: nomeDoProdutoDesejado, quantidade: quantidadeDesejadaDoProdutoDesejado, carrinho: carrinho);
                Scripts.TimeStampWriteLine($"{quantidadeDesejadaDoProdutoDesejado} x {nomeDoProdutoDesejado} foram para o carrinho.");

                nomeDoProdutoDesejado = "";
                quantidadeDesejadaDoProdutoDesejado = 0;
            }

        }
        void AdicionarProdutoAoCarrinho(string nomeDoProduto, int quantidade, Dictionary<int, Dictionary<string, Produto>> carrinho)
        {
            string nomeDoProdutoDesejado = nomeDoProduto;
            int quantidadeDesejadaDoProdutoDesejado = quantidade;
            if (carrinho.First().Value.TryGetValue(nomeDoProdutoDesejado, out Produto produto))
            {
                produto.Quantidade = quantidadeDesejadaDoProdutoDesejado;
                return;
            }
            Produto novoProduto = Scripts.CriarItemDeCarrinho(nomeDoProdutoDesejado, quantidade);
            carrinho.First().Value[nomeDoProdutoDesejado] = novoProduto;
        }

    }
}
