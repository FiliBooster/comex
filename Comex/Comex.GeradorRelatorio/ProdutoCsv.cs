using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comex.GeradorRelatorio
{
    /// <summary>
    /// Representa um produto com identificador, preço unitário, quantidade opcional e descrição.
    /// </summary>
    [CsvRecord]
    public partial class ProdutoCsv
    {
        /// <summary>
        /// Obtém ou define o nome do produto.
        /// </summary>
        [CsvColumn("Nome")]
        public string Nome { get; set; } = string.Empty;
        /// <summary>
        /// Obtém ou define o preço unitário do produto.
        /// </summary>
        [CsvColumn("Preço Unitário")]
        public decimal Unitario { get; set; }
        /// <summary>
        /// Obtém ou define a quantidade do produto, quando informada.
        /// </summary>
        [CsvColumn("Quantidade")]
        public int? Quantidade { get; set; }
        /// <summary>
        /// Obtém ou define a descrição do produto.
        /// </summary>
        [CsvColumn("Descrição")]
        public string Descricao { get; set; } = string.Empty;
    }
}
