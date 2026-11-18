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
            Console.WriteLine("\nDigite o CPF do cliente a ser cadastrado (11 dígitos):");

            if (!CpfValidation.TryNormalize(EntradaConsole.ReadLine(), out string numeroDeCpf))
            {
                Console.WriteLine("CPF inválido.");
                return;
            }

            Console.WriteLine("\nDigite o nome do cliente a ser cadastrado:");
            string nomeDoCliente = EntradaConsole.ReadLine();

            State.CadastroDeClientes[numeroDeCpf] = nomeDoCliente;
            Console.WriteLine($"\nUsuário {nomeDoCliente} ({numeroDeCpf}) cadastrado com sucesso.");
        }
    }
}
