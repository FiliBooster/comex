using Comex.GeradorRelatorio.Interfaces;
using CsvHelper;

namespace Comex.GeradorRelatorio
{
    /// <summary>
    /// Gera arquivos CSV a partir dos dados de produtos do relatório.
    /// </summary>
    public class GeradorCsv:IGeradorRelatorio
    {
        /// <summary>
        /// Exporta os produtos do relatório para um arquivo CSV.
        /// </summary>
        /// <param name="produtos">Produtos do relatório a exportar.</param>
        /// <param name="caminhoArquivo">Caminho do arquivo CSV a ser criado.</param>
        /// <returns>Caminho completo do arquivo CSV gerado.</returns>
        public static string Exportar(IEnumerable<ProdutoRelatorioDto> produtos, string caminhoArquivo)
        {
            IEnumerable<ProdutoCsv> produtosCsv = GerarProdutosCsv(produtos);
            bool gerou = GerarCsv(produtosCsv, caminhoArquivo);
            if (!gerou) { throw new FalhaAoGerarOuSalvarException(caminhoArquivo); }

            return Path.GetFullPath(caminhoArquivo);
        }

        /// <summary>
        /// Converte os produtos do relatório para registros CSV.
        /// </summary>
        /// <param name="produtos">Produtos do relatório a converter.</param>
        /// <returns>Registros CSV correspondentes aos produtos fornecidos.</returns>
        public static IEnumerable<ProdutoCsv> GerarProdutosCsv (IEnumerable<ProdutoRelatorioDto> produtos)
        {
            List<ProdutoCsv> produtosCsv = [];
            foreach (var tuplaProduto in produtos)
            {
                ProdutoCsv produtoNovoCsv = new ()
                {
                    Nome = tuplaProduto.Nome,
                    Unitario = tuplaProduto.Unitario,
                    Quantidade = tuplaProduto.Quantidade,
                    Descricao = tuplaProduto.Descricao
                };
                produtosCsv.Add(produtoNovoCsv);
            }
            return produtosCsv;

        }

        private static bool GerarCsv(IEnumerable<ProdutoCsv> produtosCsv, string caminhoArquivo)
        {
            CsvHelper.CsvWriterOptions wOptions = new() { WriteByteOrderMark = true };
            using var writer = CsvWriter.ToFile(caminhoArquivo, wOptions);
            writer.WriteRecords(produtosCsv);
            return true;
        }
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="FalhaAoGerarOuSalvarException"/>.
    /// </summary>
    /// <param name="mensagemDeErro">Mensagem de erro que descreve a falha na operação do CSV.</param>
    public class FalhaAoGerarOuSalvarException(string mensagemDeErro)
        : Exception($"Não foi gerado ou salvo o .csv em {mensagemDeErro} not found.")
    {
    }
}
