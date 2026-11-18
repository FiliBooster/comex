using System.Reflection;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal class PrincipalMenu : Menu
    {
        private readonly Dictionary<int, Menu> _itens;
        private bool _executando = true;

        internal override string Titulo => "Menu Principal";

        public PrincipalMenu(ComexState state, ScriptsComex scripts) : base(state, scripts)
        {
            _itens = DescobrirMenus(state, scripts);
        }

        public override void Executar()
        {
            while (_executando)
            {
                ExibirMenuDeOpcoes();
                if (!int.TryParse(EntradaConsole.ReadLine(), out int opcaoEscolhida))
                {
                    Console.WriteLine("\nOpção inválida. Pressione qualquer tecla para tentar novamente...");
                    EntradaConsole.ReadKey(intercept: true);
                    continue;
                }
                RotearOpcaoEscolhida(opcaoEscolhida);
            }
        }

        private void ExibirMenuDeOpcoes()
        {
            RenderizarTitulo();
            Console.WriteLine("\nDigite o número de uma das opções abaixo, para escolhê-la:");
            foreach (var item in _itens)
            {
                Console.WriteLine($"{item.Key}: {item.Value.Titulo}");
            }
            Console.WriteLine("-1: Finalizar COMEX");
        }

        private void RotearOpcaoEscolhida(int opcaoEscolhida)
        {
            if (opcaoEscolhida == -1)
            {
                _executando = false;
                Console.WriteLine("COMEX FINALIZADO");
                return;
            }

            if (_itens.TryGetValue(opcaoEscolhida, out Menu menu))
            {
                menu.RenderizarTitulo();
                menu.Executar();
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal...");
                EntradaConsole.ReadKey(intercept: true);
            }
            else
            {
                Console.WriteLine("\nOpção inválida.");
            }
        }

        private static Dictionary<int, Menu> DescobrirMenus(ComexState state, ScriptsComex scripts)
        {
            Type menuBase = typeof(Menu);

            var menus = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => menuBase.IsAssignableFrom(t) && !t.IsAbstract && t != typeof(PrincipalMenu))
                .Select(t => Activator.CreateInstance(t, new object[] { state, scripts }) as Menu)
                .Where(m => m is not null)!
                .OrderBy(m => m.Ordem)
                .ToList();

            return menus
                .Select((menu, index) => new { Key = index + 1, Menu = menu })
                .ToDictionary(x => x.Key, x => x.Menu);
        }
    }
}
