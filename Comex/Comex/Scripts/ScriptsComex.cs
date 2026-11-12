using Comex.Core;
using Comex.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Comex.Scripts
{
    internal class ScriptsComex
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string UrlApiExternaDeProdutos = "https://fakestoreapi.com/products";
        private List<ProdutoExternoDto> _cacheProdutosExternos;

        private readonly ComexState _state;
        public ScriptsComex(ComexState state)
        {
            _state = state;
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
        }
        private Dictionary<int, string> CadastroDeClientes => _state.CadastroDeClientes;
        private Dictionary<string, Produto> CadastroDeProdutos => _state.CadastroDeProdutos;
        private Dictionary<int, Dictionary<string, Produto>> CarrinhosEmAberto => _state.CarrinhosEmAberto;

        public Produto PesquisarPorProdutoNoCadastro(string nomeDoProduto)
        {
            return CadastroDeProdutos[nomeDoProduto];
        }


        public Produto CriarItemDeCarrinho(string nomeProduto, int quantidade)
        {
            Produto p = CadastroDeProdutos[nomeProduto];
            return new Produto(p.Nome, p.Descricao, p.Unitario)
            {
                Quantidade = quantidade
            };
        }

        public (int Cpf, bool TemCadastro) VerificarSeClienteJaTemCadastro()
        {
            Console.WriteLine("\nQual cliente está comprando (digite o CPF dele)?");
            if (!int.TryParse(Console.ReadLine(), out int cpf))
            {
                Console.WriteLine("CPF inválido.");
                return (cpf, false);
            }
            if (CadastroDeClientes.ContainsKey(cpf) == false)
            {
                Console.WriteLine($"Cliente não cadastrado.");
                return (cpf, false);
            }
            return (cpf, true);
        }
        public (bool TemCadastro, string nomeDoProdutoDesejado) VerificarSeProdutoJaTemCadastro()
        {
            bool TemCadastro = false;
            Console.WriteLine("Qual produto deseja adicionar ao carrinho?");
            string nomeDoProdutoDesejado = Console.ReadLine();

            if (CadastroDeProdutos.ContainsKey(key: nomeDoProdutoDesejado))
            {
                return (true, nomeDoProdutoDesejado);
            }

            ProdutoExternoDto produtoExterno = BuscarProdutoNaApiExternaPorNomeAsync(nomeDoProdutoDesejado).GetAwaiter().GetResult();
            if (produtoExterno is not null)
            {
                Produto produtoImportado = produtoExterno.ParaProduto();
                CadastroDeProdutos[produtoExterno.Title] = produtoImportado;
                TimeStampWriteLine($"Produto \"{produtoExterno.Title}\" importado automaticamente da API externa.");
                return (true, produtoExterno.Title);
            }

            Console.WriteLine("Produto não cadastrado.");
            return (TemCadastro, nomeDoProdutoDesejado);
        }

        private async Task<ProdutoExternoDto> BuscarProdutoNaApiExternaPorNomeAsync(string nomeBuscado)
        {
            if (string.IsNullOrWhiteSpace(nomeBuscado))
            {
                return null;
            }

            try
            {
                _cacheProdutosExternos ??= await _client.GetFromJsonAsync<List<ProdutoExternoDto>>(UrlApiExternaDeProdutos);

                return _cacheProdutosExternos?.FirstOrDefault(p =>
                    p.Title is not null &&
                    p.Title.Contains(nomeBuscado, StringComparison.OrdinalIgnoreCase));
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Falha ao acessar a API externa: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"A consulta à API externa excedeu o tempo limite: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Falha ao interpretar a resposta da API externa: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao consultar a API externa: {ex.Message}");
            }

            return null;
        }
        public Dictionary<int, Dictionary<string,Produto>> ObterCarrinho(int cpfDoClienteCadastrado)
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
        public Dictionary<int, Dictionary<string, Produto>> CriarCarrinho(int cpfDoClienteCadastrado)
        {
            Dictionary<int, Dictionary<string, Produto>> carrinho = new() { [cpfDoClienteCadastrado] = new()};
            RegistrarCarrinho(carrinho);
            return carrinho;

        }
        public bool RegistrarCarrinho(Dictionary<int, Dictionary<string, Produto>> carrinho)
        {
            int cpf = carrinho.Keys.First();
            CarrinhosEmAberto[cpf] = carrinho[cpf];
            return true;
        }

        public void TimeStampWriteLine(string msg)
        {
            string timestamp = System.DateTime.Now.ToLocalTime().ToString("o");
            Console.WriteLine($"{timestamp}: {msg}");
        }

    }
}
