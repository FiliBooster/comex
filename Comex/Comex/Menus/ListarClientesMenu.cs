using System;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class ListarClientesMenu : Menu
    {
        public ListarClientesMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Listar Clientes";

        public override void Executar()
        {
            Scripts.TimeStampWriteLine($"\nLista de clientes cadastrados agora:");
            int indice = 0;
            foreach (var cliente in State.CadastroDeClientes)
            {
                Console.WriteLine($"{indice}: CPF nº: {cliente.Key} | Nome: {cliente.Value}");
                indice++;
            }
        }
    }
}
