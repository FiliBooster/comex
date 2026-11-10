using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comex.Scripts
{
    internal class ScriptsComex
    {
        static ScriptsComex()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
        }

        private static readonly Dictionary<int, (string Mensagem, Action ExecutarOpcao)> OpcoesDoMenu = new()
        {
            [1] = ("Cadastrar cliente", CadastrarCliente),
            [2] = ("Listar clientes", ListarClientes),
            [3] = ("Cadastrar produto", CadastrarProduto),
            [4] = ("Listar produtos", ListarProdutos),
            [5] = ("Ajustar preco de produto", AjustarPrecoDeProduto),
            [6] = ("Adicionar produto no carrinho", AdicionarProdutosAoCarrinho),
            [7] = ("Exibir carrinho", ExibirCarrinho),
            [-1] = ("Finalizar COMEX", FinalizarComex),
        };
        private static bool _executando = true;
        private static Dictionary<int, string>? _cadastroDeClientes;
        private static Dictionary<int, string> CadastroDeClientes => _cadastroDeClientes ??= new();
        private static Dictionary<string, Produto>? _cadastroDeProdutos;
        private static Dictionary<string, Produto> CadastroDeProdutos => _cadastroDeProdutos ??= new();
        private static Dictionary<int, Dictionary<string, Produto>>? _carrinhosEmAberto;
        private static Dictionary<int, Dictionary<string,Produto>> CarrinhosEmAberto => _carrinhosEmAberto ??= new();

        public static void Executar()
        {
            while (_executando){
                ExibirMenuDeOpcoes();
                int opcaoEscolhida = ObterOpcaoEscolhida();
                RotearOpcaoEscolhida(opcaoEscolhida);
            }
        }

        static void ExibirMenuDeOpcoes()
        {
            string mensagemEscolha = "\nDigite o número de uma das opções abaixo, para escolhê-la:";
            Console.WriteLine(mensagemEscolha);
            foreach (var opcao in OpcoesDoMenu)
            {
                Console.WriteLine($"{opcao.Key}: {opcao.Value.Mensagem}");
            }
        }
        static int ObterOpcaoEscolhida()
        {
            int opcaoEscolhida = int.Parse(Console.ReadLine());
            return opcaoEscolhida;
        }
        static void RotearOpcaoEscolhida(int opcaoEscolhida)
        {
            if (OpcoesDoMenu.TryGetValue(opcaoEscolhida, out var opcao) == true)
            {
                opcao.ExecutarOpcao();
            }
            else
            {
                Console.WriteLine("\nOpção inválida.");
                Executar();
            }
        }

        static void CadastrarCliente()
        {
            Console.WriteLine("\nDigite apenas dígitos numéricos do CPF do cliente a ser cadastrado:");

            int numeroDeCpf = int.Parse(Console.ReadLine());

            Console.WriteLine("\nDigite o nome do cliente a ser cadastrado:");
            string nomeDoCliente = Console.ReadLine();

            CadastroDeClientes[numeroDeCpf] = nomeDoCliente;
            Console.WriteLine($"\nUsuário {nomeDoCliente} ({numeroDeCpf}) cadastrado com sucesso.");
            Executar();
        }

        static void ListarClientes()
        {
            TimeStampWriteLine($"\nLista de clientes cadastrados agora:");
            int indice = 0;
            foreach (var cliente in CadastroDeClientes)
            {
                Console.WriteLine($"{indice}: CPF nº: {cliente.Key} | Nome: {cliente.Value}");
                indice++;
            }
            Executar();
        }

        static void CadastrarProduto()
        {
            Console.WriteLine("\nDigite o nome do produto:");
            string nomeDoProduto = Console.ReadLine();

            Console.WriteLine("\nDigite o preço (em R$) do produto:");
            float precoDoProduto = float.Parse(Console.ReadLine(), System.Globalization.NumberStyles.AllowDecimalPoint);
            
            Console.WriteLine("\nDigite a descrição do produto:");
            string descricaoDoProduto = Console.ReadLine();

            Produto novoProduto = new Produto { Nome = nomeDoProduto, Descricao = descricaoDoProduto, Unitario = precoDoProduto };

            CadastroDeProdutos[nomeDoProduto] = novoProduto;

            TimeStampWriteLine($"Produto {nomeDoProduto} cadastrado com sucesso.");
            Executar();

        }

        static void ListarProdutos()
        {
            TimeStampWriteLine($"\nLista de produtos cadastrados agora:");
            int indice = 0;
            foreach (Produto produto in CadastroDeProdutos.Values)
            {
                Console.WriteLine($"{indice}: Nome: {produto.Nome} | Preço (R$): {produto.Unitario}");
                indice++;
            }

            Executar();
        }

        public static Produto PesquisarPorProdutoNoCadastro(string nomeDoProduto) {
            
                return CadastroDeProdutos[nomeDoProduto];
        }
        static void AjustarPrecoDeProduto()
        {
            Console.WriteLine("\nQual o nome do produto cujo preço deseja ajustar?");
            string nomeDoProduto = Console.ReadLine();
            try
            {
                Produto produto = PesquisarPorProdutoNoCadastro(nomeDoProduto);
                float precoAntigo = produto.Unitario;
                Console.WriteLine("\nDigite o novo preço (em R$) para o produto:");
                float novoPreco = float.Parse(Console.ReadLine(), System.Globalization.NumberStyles.AllowDecimalPoint);
                CadastroDeProdutos[nomeDoProduto].Unitario = novoPreco;
                TimeStampWriteLine("Preço ajustado!");
                Console.WriteLine($"[{nomeDoProduto}]:\n preço antigo: {precoAntigo} | preço novo: {novoPreco}");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"Nenhum produto encontrado com nome [{nomeDoProduto}] no Cadastro de Produtos.");
            }
            Executar();
        }

        static Produto CriarItemDeCarrinho(string nomeProduto, int quantidade)
        {
            Produto p = CadastroDeProdutos[nomeProduto];
            return new Produto
            {
                Nome = p.Nome,
                Descricao = p.Descricao,
                Unitario = p.Unitario,
                Quantidade = quantidade
            };
        }

        static void AdicionarProdutosAoCarrinho()
        {
            (int cpfDoCliente, bool clienteJaTemCadastro) = VerificarSeClienteJaTemCadastro();
            if (clienteJaTemCadastro == false){ return; }

            Dictionary<int, Dictionary<string, Produto>> carrinho = ObterCarrinho(cpfDoCliente);

            bool adicaoConcluida = false;
            bool produtoTemCadastro = false;
            string nomeDoProdutoDesejado = "";
            int quantidadeDesejadaDoProdutoDesejado = 0;
            while (adicaoConcluida == false)
            {
                (produtoTemCadastro, nomeDoProdutoDesejado) = VerificarSeProdutoJaTemCadastro();
                Console.WriteLine($"-- Digite '0' para sair.");
                if (Console.ReadKey().Key is ConsoleKey.D0) { break; }

                if (produtoTemCadastro == false) { continue; }

                Console.WriteLine($"\nQual a quantidade do produto {nomeDoProdutoDesejado} deseja adicionar ao carrinho?");
                quantidadeDesejadaDoProdutoDesejado = int.Parse(Console.ReadLine());
                if (quantidadeDesejadaDoProdutoDesejado == 0) { continue; } //se quantidade é zero, não adiciona produto

                AdicionarProdutoAoCarrinho(nomeDoProduto: nomeDoProdutoDesejado, quantidade: quantidadeDesejadaDoProdutoDesejado, carrinho: carrinho);
                TimeStampWriteLine($"{quantidadeDesejadaDoProdutoDesejado} x {nomeDoProdutoDesejado} foram para o carrinho.");

                nomeDoProdutoDesejado = "";
                quantidadeDesejadaDoProdutoDesejado = 0;
            }
            Executar();

        }

        static void AdicionarProdutoAoCarrinho(string nomeDoProduto, int quantidade, Dictionary<int, Dictionary<string,Produto>> carrinho)
        {
            string nomeDoProdutoDesejado = nomeDoProduto;
            int quantidadeDesejadaDoProdutoDesejado = quantidade;
            if (carrinho.First().Value.TryGetValue(nomeDoProdutoDesejado, out Produto produto))
            {
                produto.Quantidade = quantidadeDesejadaDoProdutoDesejado;
                return;
            }
            Produto novoProduto = CriarItemDeCarrinho(nomeDoProdutoDesejado, quantidade);
            carrinho.First().Value[nomeDoProdutoDesejado]=novoProduto;
        }

        static (int Cpf, bool TemCadastro) VerificarSeClienteJaTemCadastro()
        {
            Console.WriteLine("\nQual cliente está comprando (digite o CPF dele)?");
            int cpf = int.Parse(Console.ReadLine());
            if (CadastroDeClientes.ContainsKey(cpf) == false)
            {
                Console.WriteLine($"Cliente não cadastrado.");
                return (cpf, false);
            }
            return (cpf, true);
        }
        static (bool TemCadastro, string nomeDoProdutoDesejado) VerificarSeProdutoJaTemCadastro() 
        {
            bool TemCadastro = false;
            Console.WriteLine("Qual produto deseja adicionar ao carrinho?");
            string nomeDoProdutoDesejado = Console.ReadLine();
                if (CadastroDeProdutos.ContainsKey(key: nomeDoProdutoDesejado) == false)
                {
                    Console.WriteLine("Produto não cadastrado.");
                }
                else TemCadastro = true;
            return (TemCadastro, nomeDoProdutoDesejado);
        }
        static Dictionary<int, Dictionary<string,Produto>> ObterCarrinho(int cpfDoClienteCadastrado)
        {
            Dictionary<int, Dictionary<string, Produto>> carrinho = new();
            if (CarrinhosEmAberto.TryGetValue(key: cpfDoClienteCadastrado, out var itensDeCarrinho)) {
                carrinho[cpfDoClienteCadastrado] = itensDeCarrinho;
            }
            else 
            {
                carrinho = CriarCarrinho(cpfDoClienteCadastrado);
            }
            return carrinho;
        }
        static Dictionary<int, Dictionary<string, Produto>> CriarCarrinho(int cpfDoClienteCadastrado)
        {
            Dictionary<int, Dictionary<string, Produto>> carrinho = new() { [cpfDoClienteCadastrado] = new()};
            RegistrarCarrinho(carrinho);
            return carrinho;

        }
        static bool RegistrarCarrinho(Dictionary<int, Dictionary<string, Produto>> carrinho)
        {
            int cpf = carrinho.Keys.First();
            CarrinhosEmAberto[cpf] = carrinho[cpf];
            return true;
        }

        static void ExibirCarrinho()
        {
            Console.WriteLine("\nDigite o nº do CPF do cliente cujo carrinho deseja exibir:");
            int cpfDoCliente = int.Parse(Console.ReadLine());
            foreach (Produto produto in CarrinhosEmAberto[cpfDoCliente].Values)
            {
                float subtotal = produto.Quantidade * produto.Unitario;
                Console.WriteLine($"Produto: {produto.Nome} | Quantidade: {produto.Quantidade} | Preço Unitário (R$): {produto.Unitario} | Sub-total (R$): {subtotal} ");
            }
            Executar();
        }

        static void FinalizarComex()
        {
            _executando = false;
            Console.WriteLine("COMEX FINALIZADO");
        }

        static void TimeStampWriteLine(string msg)
        {
            string timestamp = System.DateTime.Now.ToLocalTime().ToString("o");
            Console.WriteLine($"{timestamp}: {msg}");
        }

    }
}
