using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comex;

namespace Comex.Modelos
{
    public class ProdutoExternoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public RatingDto Rating { get; set; }

        /// <summary>
        /// Converte este produto vindo da API externa para o modelo interno <see cref="Produto"/>
        /// utilizado pelo cadastro de produtos do COMEX.
        /// </summary>
        internal Produto ParaProduto()
        {
            return new Produto(
                Nome: Title,
                Descricao: Description,
                Unitario: (float)Price);
        }
    }
    public class RatingDto
    {
        public double Rate { get; set; }
        public int Count { get; set; }
    }
}
