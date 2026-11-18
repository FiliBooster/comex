# comex

## Console

No console clássico do Windows, o Comex traz o cursor de entrada para a área visível depois de exibir um menu ou uma solicitação de entrada, sem limpar o histórico. Se você mudar o zoom enquanto o programa aguarda uma resposta, o ajuste ocorre na próxima solicitação de entrada. Em terminais como o Windows Terminal, a rolagem do histórico é controlada pelo próprio terminal.

## CPF

O CPF é tratado como um identificador textual: a entrada é aparada de espaços externos e deve conter exatamente 11 dígitos ASCII (`0`-`9`). Zeros à esquerda são preservados. Pontuação, sinais, letras e comprimentos diferentes são rejeitados. Esta é somente uma validação de formato; não verifica dígitos verificadores nem comprova que o CPF seja válido ou registrado. Um cadastro existente com o mesmo CPF continua sendo sobrescrito.

## Relatórios CSV

No menu principal do Comex, selecione **Gerar Relatório CSV** e escolha o cadastro de produtos ou o carrinho de um cliente (informando o CPF). Informe o caminho do arquivo de destino; a pasta deve existir. Nomes sem pasta são salvos no diretório de execução do processo. O arquivo é sobrescrito quando já existe; a gravação não é atualmente atômica. O menu exibe o caminho completo somente após a exportação ser concluída ou uma mensagem de erro se não for possível salvar.

O relatório inclui nome, preço unitário, quantidade e descrição, nessa ordem. A exportação direta da biblioteca cria um CSV com cabeçalho mesmo quando a coleção está vazia; no menu, coleções vazias mantêm a mensagem de que não há produtos para exportar e não criam arquivo.

Outros menus podem exportar qualquer `IEnumerable<Produto>` usando `new GerarRelatorioMenu(State, Scripts).Executar(produtos)`, incluindo `State.CadastroDeProdutos.Values` ou os valores de um carrinho. Esse método solicita o caminho de destino ao usuário.

A biblioteca de relatórios é empacotada com o ID NuGet `Comex.GeneradorRelatorio`; o Comex usa o projeto da biblioteca por referência de projeto.
