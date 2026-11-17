# comex

## Relatórios CSV

No menu principal do Comex, selecione **Gerar Relatório CSV** e escolha o cadastro de produtos ou o carrinho de um cliente (informando o CPF). Informe o caminho do arquivo de destino; a pasta deve existir. Nomes sem pasta são salvos no diretório de execução, e arquivos existentes são substituídos. O menu exibe o caminho completo após a exportação ou uma mensagem de erro se não for possível salvar.

O relatório inclui nome, preço unitário, quantidade e descrição de cada produto. Coleções vazias não geram arquivo.

Outros menus podem exportar qualquer `IEnumerable<Produto>` usando `new GerarRelatorioMenu(State, Scripts).Executar(produtos)`, incluindo `State.CadastroDeProdutos.Values` ou os valores de um carrinho. Esse método solicita o caminho de destino ao usuário.
