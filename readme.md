# File Extractor

Este é um programa de console desenvolvido em C# que realiza a extração de arquivos .zip. O programa monitora um diretório especificado em `appsettings.json` em busca de arquivos .zip contendo a palavra "ZPL" no nome. Ao encontrar um arquivo compatível, ele é extraído para um diretório temporário, e os arquivos .txt presentes dentro do arquivo .zip são movidos para um diretório de saída especificado. O arquivo .zip original é movido para uma pasta "extraídos" dentro do diretório de saída.

## Configurações

As configurações do programa são definidas no arquivo `appsettings.json`. As seguintes configurações estão disponíveis:

```json
{
  "DirectoryIn": "Caminho/Para/Diretório/De/Entrada",
  "DirectoryOut": "Caminho/Para/Diretório/De/Saída",
  "IntervaloExecucao": 5000
}


- `DirectoryIn`: O caminho para o diretório de entrada, onde os arquivos .zip são monitorados.
- `DirectoryOut`: O caminho para o diretório de saída, onde os arquivos .txt extraídos serão movidos.
- `IntervaloExecucao`: O intervalo de tempo, em milissegundos, entre as verificações de novos arquivos .zip no diretório de entrada.

## Como Usar

1. Configure as opções do programa no arquivo `appsettings.json`, especificando o caminho para o diretório de entrada, o diretório de saída e o intervalo de execução.
2. Execute o programa.
3. O programa irá verificar periodicamente o diretório de entrada em busca de novos arquivos .zip.
4. Quando um arquivo .zip compatível for encontrado, ele será extraído para o diretório temporário.
5. Os arquivos .txt presentes dentro do arquivo .zip serão movidos para o diretório de saída especificado.
6. O arquivo .zip original será movido para a pasta "extraídos" dentro do diretório de saída.

Certifique-se de ter a versão correta do .NET instalada e execute o programa usando o comando `dotnet run` no terminal.