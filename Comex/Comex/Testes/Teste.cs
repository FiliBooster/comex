using Comex;
using Comex.Core;
using Comex.Menus;
using Comex.Scripts;

class Teste()
{
    public void Executar()
    {
    string nomeDeLivro = "Livro Bíblia King James";
    string descricaoDeLivro = "Reverenciada obra antiga de sabedoria oriental.";
    decimal precoUnitarioDeLivro = 120.00m;
    Comex.Produto produto = new Produto(nomeDeLivro, descricaoDeLivro, precoUnitarioDeLivro);

    Console.WriteLine(produto.Nome);
    Console.WriteLine(produto.Descricao);
    Console.WriteLine(produto.Unitario);

    EntradaConsole.ReadKey();

    ComexState state = new();
    ScriptsComex scripts = new(state);
    PrincipalMenu principalMenu = new(state, scripts);
    principalMenu.Executar();
    }

}
