using Comex.Core;
using Comex.Modelos;
using Comex.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Comex.Menus
{
    internal class BuscarEmApiExternaMenu : Menu
    {
        private static readonly HttpClient _client = new HttpClient();
        public BuscarEmApiExternaMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Listar Produtos Cadastrados em API Externa";

        public override void Executar()
        {
            Scripts.TimeStampWriteLine($"\nLista de produtos cadastrados em API externa agora:");
            ObterProdutosAsync().GetAwaiter().GetResult();
        }

        public async Task ObterProdutosAsync()
        {
            const string url = "https://fakestoreapi.com/products";
            try
            {
                List<ProdutoExternoDto> produtos = await _client.GetFromJsonAsync<List<ProdutoExternoDto>>(url);
                if (produtos is null || produtos.Count == 0)
                {
                    Console.WriteLine("Nenhum produto encontrado na API externa.");
                    return;
                }

                IEnumerable<ProdutoExternoDto> produtosOrdenados = OrdenarProdutos(produtos);

                foreach (ProdutoExternoDto produto in produtosOrdenados)
                {
                    Console.WriteLine($"[{produto.Id}] {produto.Title} | Categoria: {produto.Category} | Preço (R$): {produto.Price} | Avaliação: {produto.Rating?.Rate} ({produto.Rating?.Count} avaliações)");
                }

                Console.WriteLine($"\nConsulta realizada com sucesso. {produtos.Count} produto(s) encontrado(s).");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Falha ao acessar a API externa: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"A consulta à API externa excedeu o tempo limite: {ex.Message}");
            }
            catch (System.Text.Json.JsonException ex)
            {
                Console.WriteLine($"Falha ao interpretar a resposta da API externa: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao consultar a API externa: {ex.Message}");
            }
        }

        private static IEnumerable<ProdutoExternoDto> OrdenarProdutos(List<ProdutoExternoDto> produtos)
        {
            Console.WriteLine("\nComo deseja ordenar os produtos?");
            Console.WriteLine("1: Título (A-Z)");
            Console.WriteLine("2: Título (Z-A)");
            Console.WriteLine("3: Preço (menor para maior)");
            Console.WriteLine("4: Preço (maior para menor)");
            Console.WriteLine("Qualquer outra tecla: manter ordem original");

            string opcao = Console.ReadLine();

            return opcao switch
            {
                "1" => produtos.OrderBy(p => p.Title, StringComparer.OrdinalIgnoreCase),
                "2" => produtos.OrderByDescending(p => p.Title, StringComparer.OrdinalIgnoreCase),
                "3" => produtos.OrderBy(p => p.Price),
                "4" => produtos.OrderByDescending(p => p.Price),
                _ => produtos
            };
        }
    }
}
