using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comex.GeradorRelatorio
{
    /// <summary>
    /// Representa os dados de um produto usados na geração do relatório.
    /// </summary>
    public class ProdutoRelatorioDto
    {
        /// <summary>
        /// Obtém ou define o nome do produto.
        /// </summary>
        public required string Nome { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do produto.
        /// </summary>
        public required string Descricao { get; set; }

        /// <summary>
        /// Obtém ou define o preço unitário do produto.
        /// </summary>
        public required decimal Unitario { get; set; }

        /// <summary>
        /// Obtém ou define a quantidade do produto.
        /// </summary>
        public int Quantidade { get; set; }
    }
}
