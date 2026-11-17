using Comex.GeradorRelatorio;

var produtos = new[]
{
    new ProdutoRelatorioDto
    {
        Nome = "Caneta",
        Descricao = "Azul",
        Unitario = 2.50m,
        Quantidade = 3
    }
};

var arquivo = Path.Combine(Path.GetTempPath(), "comex-smoke-test.csv");
var caminhoGerado = GeradorCsv.Exportar(produtos, arquivo);

if (!File.Exists(caminhoGerado))
    throw new Exception("CSV was not created.");

var csv = File.ReadAllText(caminhoGerado);
if (!csv.Contains("Caneta") || !csv.Contains("Azul"))
    throw new Exception("CSV does not contain the expected product.");

Console.WriteLine($"CSV generated: {caminhoGerado}");