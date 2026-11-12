using System;
using Comex.Core;
using Comex.Scripts;

namespace Comex.Menus
{
    internal abstract class Menu
    {
        protected ComexState State { get; }
        protected ScriptsComex Scripts { get; }

        protected Menu(ComexState state, ScriptsComex scripts)
        {
            State = state;
            Scripts = scripts;
        }

        internal abstract string Titulo { get; }
        internal virtual int Ordem => int.MaxValue;
        public abstract void Executar();
        public void RenderizarTitulo()
        {
            int numeroDeCaracteres = Titulo.Trim().Length;
            string graus = string.Empty.PadLeft(numeroDeCaracteres, '°');
            Console.WriteLine(graus);
            Console.WriteLine(Titulo.Trim());
            Console.WriteLine(graus + "\n");
        }
    }
}
