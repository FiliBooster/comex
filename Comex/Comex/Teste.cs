using Comex;
using System.Runtime.CompilerServices;

class Teste()
{
    public void Executar()
    {
    string nomeDeLivro = "Livro Bíblia King James";
    string descricaoDeLivro = "Reverenciada obra antiga de sabedoria oriental.";
    Comex.Produto produto = new Produto { Nome = nomeDeLivro, Descricao = descricaoDeLivro };

    Console.WriteLine(produto.Nome);
    Console.WriteLine(produto.Descricao);
    }
}
