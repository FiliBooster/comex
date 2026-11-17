//using CsvHelper;

//List<(string Id, decimal Preco, int Quantidade, string Descricao)> listaDeTuplasDeProdutos = new()
//{
//    ("pum", 1.11m, 10, "Produto um."),
//    ("p2", 2.22m, 20, "Produto dois."),
//    ("p3", 3.33m, 30, "Produto três.")
//};

//List<Produto> produtos = new();
//foreach(var tuplaProduto in listaDeTuplasDeProdutos)
//{
//    Produto produtoNovo = new Produto { 
//        Id = tuplaProduto.Id, 
//        Unitario = tuplaProduto.Preco, 
//        Quantidade = tuplaProduto.Quantidade, 
//        Descricao = tuplaProduto.Descricao 
//    };
//    produtos.Add(produtoNovo);
//}

//CsvHelper.CsvWriterOptions wOptions = new CsvWriterOptions { WriteByteOrderMark = true };
//bool leaveOpen = false;
//using var writer = CsvWriter.ToFile("listaDeProdutos.csv", wOptions);
//writer.WriteRecords(produtos);
////CsvHelper.CsvWriter.Create(stream, wOptions, leaveOpen);

//[CsvRecord]
//public partial class Produto
//{
//    public string Id { get; set; } = string.Empty;
//    [CsvColumn("Preço Unitário")]
//    public decimal Unitario { get; set; }
//    [CsvColumn("Quantidade")]
//    public int? Quantidade { get; set; }
//    [CsvColumn("Descrição")]
//    public string Descricao {  get; set; } = string.Empty;
//}

