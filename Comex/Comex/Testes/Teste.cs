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
    float precoUnitarioDeLivro = 120.00f;
    Comex.Produto produto = new Produto(nomeDeLivro, descricaoDeLivro, precoUnitarioDeLivro);

    Console.WriteLine(produto.Nome);
    Console.WriteLine(produto.Descricao);
    Console.WriteLine(produto.Unitario);

    Console.ReadKey();

    ComexState state = new();
    ScriptsComex scripts = new(state);
    PrincipalMenu principalMenu = new(state, scripts);
    principalMenu.Executar();
    }

}
