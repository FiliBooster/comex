using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comex.GeradorRelatorio.Interfaces
{
    ///<summary>Define membros para gerador de relatório sobre relação de produtos.</summary>
    public interface IGeradorRelatorio
    {
        ///<summary>Define membros para gerador de relatório sobre relação de produtos.</summary>
        ///<param name="produtos">
        ///Os <see cref="ProdutoRelatorioDto"/> produtos listados a se incluir em relatório.
        ///</param>
        ///<param name="caminhoArquivo">
        ///Caminho absoluto que se deseja para salvamento do relatório gerado e exportado.
        ///</param>
        ///<returns>Retorna o caminho absoluto para o relatório efetivamente gerado e exportado.</returns>
        static abstract string Exportar(IEnumerable<ProdutoRelatorioDto> produtos, string caminhoArquivo);
    }
}
