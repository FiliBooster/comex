using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comex.Core
{
    internal sealed class ComexState
    {
        public Dictionary<int, string> CadastroDeClientes { get; } = new();
        public Dictionary<string, Produto> CadastroDeProdutos { get; } = new();
        public Dictionary<int, Dictionary<string, Produto>> CarrinhosEmAberto { get; } = new();
    }
}
