using Comex.Core;
using Comex.GeradorRelatorio;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class GerarRelatorioMenu : Menu
    {
        public GerarRelatorioMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Gerar Relatório CSV";

        public override void Executar()
        {
            Console.WriteLine("\nQuais produtos deseja exportar?");
            Console.WriteLine("1: Cadastro de produtos");
            Console.WriteLine("2: Carrinho de um cliente");

            switch (EntradaConsole.ReadLine())
            {
                case "1":
                    Executar(State.CadastroDeProdutos.Values);
                    break;
                case "2":
                    Console.WriteLine("\nDigite o CPF do cliente cujo carrinho deseja exportar:");
                    if (!CpfValidation.TryNormalize(EntradaConsole.ReadLine(), out string cpf))
                    {
                        Console.WriteLine("CPF inválido.");
                        return;
                    }
                    if (!State.CarrinhosEmAberto.TryGetValue(cpf, out var carrinho))
                    {
                        Console.WriteLine("Nenhum carrinho encontrado para esse cliente.");
                        return;
                    }
                    Executar(carrinho.Values);
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }

        public void Executar(IEnumerable<Produto> produtos)
        {
            ArgumentNullException.ThrowIfNull(produtos);

            var produtosRelatorio = produtos.Select(p => new ProdutoRelatorioDto
            {
                Nome = p.Nome,
                Descricao = p.Descricao,
                Unitario = p.Unitario,
                Quantidade = p.Quantidade
            }).ToList();

            if (produtosRelatorio.Count == 0)
            {
                Console.WriteLine("Nenhum produto para exportar.");
                return;
            }

            Console.WriteLine("\nDigite o caminho do arquivo CSV (ex.: C:\\Relatorios\\produtos.csv).");
            Console.WriteLine("Um nome sem pasta salva no diretório de execução. Arquivos existentes serão substituídos.");
            string? caminhoArquivo = EntradaConsole.ReadLine();
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
            {
                Console.WriteLine("Caminho do arquivo inválido.");
                return;
            }

            try
            {
                string caminhoGerado = GeradorCsv.Exportar(produtosRelatorio, caminhoArquivo);
                Scripts.TimeStampWriteLine($"Relatório salvo em: {caminhoGerado}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Falha ao salvar o relatório: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Sem permissão para salvar o relatório: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Caminho do arquivo inválido: {ex.Message}");
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Caminho do arquivo não suportado: {ex.Message}");
            }
        }
    }
}
