using System.Text;
using Comex;
using Comex.Core;
using Comex.GeradorRelatorio;
using Comex.Menus;
using Comex.Scripts;

namespace Comex.Tests;

[CollectionDefinition("Console tests", DisableParallelization = true)]
public sealed class ConsoleTestsCollection
{
}

[Collection("Console tests")]
public sealed class ComexRegressionTests
{
    [Fact]
    public void GerarProdutosCsvMapsAllFields()
    {
        var source = CreateReportProduct();

        var result = Assert.Single(GeradorCsv.GerarProdutosCsv([source]));

        Assert.Equal(source.Nome, result.Nome);
        Assert.Equal(source.Descricao, result.Descricao);
        Assert.Equal(source.Unitario, result.Unitario);
        Assert.Equal(source.Quantidade, result.Quantidade);
    }

    [Fact]
    public void ExportarWritesUtf8BomAndEscapedCsvRecords()
    {
        var directory = CreateTemporaryDirectory();
        var file = Path.Combine(directory, "catalogo.csv");
        var products = new[]
        {
            new ProdutoRelatorioDto
            {
                Nome = "Café, \"Especial\"",
                Descricao = "Torra clara\nOrigem: Brasil",
                Unitario = 12.50m,
                Quantidade = 3
            }
        };

        try
        {
            var fullPath = GeradorCsv.Exportar(products, file);
            var bytes = File.ReadAllBytes(fullPath);
            var text = Encoding.UTF8.GetString(bytes.AsSpan(3));

            Assert.Equal(Path.GetFullPath(file), fullPath);
            Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, bytes[..3]);
            var quote = "\"";
            var expected = $"Nome,Preço Unitário,Quantidade,Descrição\r\n{quote}Café, {quote}{quote}Especial{quote}{quote}{quote},12.50,3,{quote}Torra clara\nOrigem: Brasil{quote}\r\n";
            Assert.Equal(expected, text);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void ExportarWritesHeaderOnlyForEmptyCollection()
    {
        var directory = CreateTemporaryDirectory();
        var file = Path.Combine(directory, "vazio.csv");

        try
        {
            GeradorCsv.Exportar(Array.Empty<ProdutoRelatorioDto>(), file);

            var bytes = File.ReadAllBytes(file);
            Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, bytes[..3]);
            Assert.Equal("Nome,Preço Unitário,Quantidade,Descrição\r\n", Encoding.UTF8.GetString(bytes.AsSpan(3)));
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void ExportarOverwritesAnExistingFile()
    {
        var directory = CreateTemporaryDirectory();
        var file = Path.Combine(directory, "catalogo.csv");
        File.WriteAllText(file, "conteúdo anterior");

        try
        {
            GeradorCsv.Exportar([CreateReportProduct()], file);

            var bytes = File.ReadAllBytes(file);
            var text = Encoding.UTF8.GetString(bytes.AsSpan(3));
            Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, bytes[..3]);
            Assert.StartsWith("Nome,Preço Unitário,Quantidade,Descrição\r\nCafé,12.50,3,Grãos especiais", text);
            Assert.DoesNotContain("conteúdo anterior", text);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void ExportarRejectsNullProducts()
    {
        Assert.Throws<ArgumentNullException>(() => GeradorCsv.Exportar(null!, "produtos.csv"));
        Assert.Throws<ArgumentNullException>(() => GeradorCsv.GerarProdutosCsv(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void ExportarRejectsBlankPath(string path)
    {
        Assert.Throws<ArgumentException>(() => GeradorCsv.Exportar(Array.Empty<ProdutoRelatorioDto>(), path));
    }

    [Fact]
    public void ExportarPreservesNativeFileExceptions()
    {
        var directory = CreateTemporaryDirectory();
        var file = Path.Combine(directory, "missing", "products.csv");

        try
        {
            Assert.Throws<DirectoryNotFoundException>(() => GeradorCsv.Exportar([CreateReportProduct()], file));
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void CadastroTrimsAndPreservesLeadingZeroCpf()
    {
        var state = new ComexState();
        var scripts = new ScriptsComex(state);
        var output = RunWithConsole(" 00123456789 \nAna\n", () => new CadastrarClienteMenu(state, scripts).Executar());

        Assert.Equal("Ana", state.CadastroDeClientes["00123456789"]);
        Assert.Contains("00123456789", output);
        Assert.DoesNotContain("CPF inválido", output);
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("123.456.789-01")]
    [InlineData("+1234567890")]
    [InlineData("1234567890a")]
    [InlineData("１２３４５６７８９０１")]
    public void CadastroRejectsMalformedCpfWithoutMutation(string cpf)
    {
        var state = new ComexState();
        var scripts = new ScriptsComex(state);
        var output = RunWithConsole($"{cpf}\n", () => new CadastrarClienteMenu(state, scripts).Executar());

        Assert.Empty(state.CadastroDeClientes);
        Assert.Contains("CPF inválido", output);
    }

    [Fact]
    public void CadastroContinuesToOverwriteAnExistingCustomer()
    {
        var state = new ComexState();
        var stateScripts = new ScriptsComex(state);
        state.CadastroDeClientes["00123456789"] = "Antes";

        RunWithConsole("00123456789\nDepois\n", () => new CadastrarClienteMenu(state, stateScripts).Executar());

        Assert.Equal("Depois", state.CadastroDeClientes["00123456789"]);
        Assert.Single(state.CadastroDeClientes);
    }

    [Fact]
    public void ClienteIdentificationPreservesLeadingZeroCpf()
    {
        var cliente = new Cliente
        {
            Nome = "Ana",
            CPF = "00123456789",
            Email = "ana@example.test",
            Profissao = "Analista",
            Telefone = 123456,
            Endereco = "Rua Central"
        };

        Assert.Equal("nome: Ana | CPF: 00123456789", cliente.Identificar());
    }

    [Fact]
    public void InvalidCpfDoesNotCreateCartWhenStartingPurchase()
    {
        var state = new ComexState();
        var scripts = new ScriptsComex(state);

        var output = RunWithConsole("123.456.789-01\n", () => new AdicionarProdutosAoCarrinhoMenu(state, scripts).Executar());

        Assert.Contains("CPF inválido", output);
        Assert.Empty(state.CarrinhosEmAberto);
    }

    [Fact]
    public void LeadingZeroCpfCreatesRetrievesDisplaysAndExportsSameCart()
    {
        const string cpf = "00123456789";
        var directory = CreateTemporaryDirectory();
        var file = Path.Combine(directory, "carrinho.csv");
        var state = new ComexState();
        var scripts = new ScriptsComex(state);
        state.CadastroDeClientes[cpf] = "Ana";
        state.CadastroDeProdutos["Café"] = new Produto("Café", "Torra clara", 12.50m);
        var created = scripts.ObterCarrinho(cpf);
        created[cpf]["Café"] = scripts.CriarItemDeCarrinho("Café", 3);

        try
        {
            var retrieved = scripts.ObterCarrinho(cpf);
            Assert.Same(created[cpf], retrieved[cpf]);

            var displayOutput = RunWithConsole($" {cpf} \n", () => new ExibirCarrinhoMenu(state, scripts).Executar());
            Assert.Contains("Produto: Café | Quantidade: 3", displayOutput);

            var reportOutput = RunWithConsole($"2\n{cpf}\n{file}\n", () => new GerarRelatorioMenu(state, scripts).Executar());
            var exported = Encoding.UTF8.GetString(File.ReadAllBytes(file).AsSpan(3));
            Assert.Contains(Path.GetFullPath(file), reportOutput);
            Assert.Contains("Café", exported);
            Assert.Contains("12.50", exported);
            Assert.Equal(new[] { cpf }, state.CarrinhosEmAberto.Keys);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void DisplayingMissingOrEmptyCartDoesNotCreateAnotherCart()
    {
        const string existingCpf = "00123456789";
        var state = new ComexState();
        var scripts = new ScriptsComex(state);
        state.CarrinhosEmAberto[existingCpf] = new Dictionary<string, Produto>();

        var missingOutput = RunWithConsole("00987654321\n", () => new ExibirCarrinhoMenu(state, scripts).Executar());
        Assert.Contains("Nenhum carrinho encontrado", missingOutput);
        Assert.Single(state.CarrinhosEmAberto);

        var emptyOutput = RunWithConsole($"{existingCpf}\n", () => new ExibirCarrinhoMenu(state, scripts).Executar());
        Assert.Contains("Carrinho vazio", emptyOutput);
        Assert.Single(state.CarrinhosEmAberto);
    }

    [Fact]
    public void ExportingEmptyCartPreservesMenuMessageWithoutCreatingCart()
    {
        const string cpf = "00123456789";
        var state = new ComexState();
        var scripts = new ScriptsComex(state);
        state.CarrinhosEmAberto[cpf] = new Dictionary<string, Produto>();

        var output = RunWithConsole($"2\n{cpf}\n", () => new GerarRelatorioMenu(state, scripts).Executar());

        Assert.Contains("Nenhum produto para exportar", output);
        Assert.DoesNotContain("Relatório salvo em:", output);
        Assert.Single(state.CarrinhosEmAberto);
    }

    [Fact]
    public void CatalogExportWritesCsvThroughComexMenu()
    {
        var directory = CreateTemporaryDirectory();
        var file = Path.Combine(directory, "catalogo.csv");
        var state = new ComexState();
        var scripts = new ScriptsComex(state);
        state.CadastroDeProdutos["Café"] = new Produto("Café", "Torra clara", 12.50m);

        try
        {
            var output = RunWithConsole($"1\n{file}\n", () => new GerarRelatorioMenu(state, scripts).Executar());
            var text = Encoding.UTF8.GetString(File.ReadAllBytes(file).AsSpan(3));

            Assert.Contains(Path.GetFullPath(file), output);
            Assert.Contains("Relatório salvo em:", output);
            Assert.Contains("Café", text);
            Assert.Contains("Torra clara", text);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void ExportFailureDoesNotReportSuccess()
    {
        var directory = CreateTemporaryDirectory();
        var file = Path.Combine(directory, "missing", "catalogo.csv");
        var state = new ComexState();
        var scripts = new ScriptsComex(state);
        state.CadastroDeProdutos["Café"] = new Produto("Café", "Torra clara", 12.50m);

        try
        {
            var output = RunWithConsole($"1\n{file}\n", () => new GerarRelatorioMenu(state, scripts).Executar());

            Assert.Contains("Falha ao salvar o relatório", output);
            Assert.DoesNotContain("Relatório salvo em:", output);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static ProdutoRelatorioDto CreateReportProduct() => new()
    {
        Nome = "Café",
        Descricao = "Grãos especiais",
        Unitario = 12.50m,
        Quantidade = 3
    };

    private static string RunWithConsole(string inputText, Action action)
    {
        var originalInput = Console.In;
        var originalOutput = Console.Out;
        using var input = new StringReader(inputText);
        using var output = new StringWriter();

        try
        {
            Console.SetIn(input);
            Console.SetOut(output);
            action();
            return output.ToString();
        }
        finally
        {
            Console.SetIn(originalInput);
            Console.SetOut(originalOutput);
        }
    }

    private static string CreateTemporaryDirectory()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"comex-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        return directory;
    }
}
