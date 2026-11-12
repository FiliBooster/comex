using System;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class CadastrarClienteMenu : Menu
    {
        public CadastrarClienteMenu(ComexState state, ScriptsComex scripts) : base(state, scripts) { }

        internal override string Titulo => "Cadastrar Cliente";

        public override void Executar()
        {
            Console.WriteLine("\nDigite apenas dígitos numéricos do CPF do cliente a ser cadastrado:");

            if (!int.TryParse(Console.ReadLine(), out int numeroDeCpf))
            {
                Console.WriteLine("CPF inválido.");
                return;
            }

            Console.WriteLine("\nDigite o nome do cliente a ser cadastrado:");
            string nomeDoCliente = Console.ReadLine();

            State.CadastroDeClientes[numeroDeCpf] = nomeDoCliente;
            Console.WriteLine($"\nUsuário {nomeDoCliente} ({numeroDeCpf}) cadastrado com sucesso.");
        }
    }
}
