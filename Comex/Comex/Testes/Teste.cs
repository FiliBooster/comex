using Comex;
using Comex.Scripts;
using System.Runtime.CompilerServices;

class Teste()
{
    public void Executar()
    {
    string nomeDeLivro = "Livro Bíblia King James";
    string descricaoDeLivro = "Reverenciada obra antiga de sabedoria oriental.";
    float precoUnitarioDeLivro = 120.00f;
    Comex.Produto produto = new Produto { Nome = nomeDeLivro, Descricao = descricaoDeLivro , Unitario = precoUnitarioDeLivro};

    Console.WriteLine(produto.Nome);
    Console.WriteLine(produto.Descricao);
    Console.WriteLine(produto.Unitario);

    Console.ReadKey();
    ScriptsComex.Executar();
    }

}
