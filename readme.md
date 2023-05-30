# File Extractor

Este � um programa de console desenvolvido em C# que realiza a extra��o de arquivos .zip. O programa monitora um diret�rio especificado em `appsettings.json` em busca de arquivos .zip contendo a palavra "ZPL" no nome. Ao encontrar um arquivo compat�vel, ele � extra�do para um diret�rio tempor�rio, e os arquivos .txt presentes dentro do arquivo .zip s�o movidos para um diret�rio de sa�da especificado. O arquivo .zip original � movido para uma pasta "extra�dos" dentro do diret�rio de sa�da.

## Configura��es

As configura��es do programa s�o definidas no arquivo `appsettings.json`. As seguintes configura��es est�o dispon�veis:

```json
{
  "DirectoryIn": "Caminho/Para/Diret�rio/De/Entrada",
  "DirectoryOut": "Caminho/Para/Diret�rio/De/Sa�da",
  "IntervaloExecucao": 5000
}


- `DirectoryIn`: O caminho para o diret�rio de entrada, onde os arquivos .zip s�o monitorados.
- `DirectoryOut`: O caminho para o diret�rio de sa�da, onde os arquivos .txt extra�dos ser�o movidos.
- `IntervaloExecucao`: O intervalo de tempo, em milissegundos, entre as verifica��es de novos arquivos .zip no diret�rio de entrada.

## Como Usar

1. Configure as op��es do programa no arquivo `appsettings.json`, especificando o caminho para o diret�rio de entrada, o diret�rio de sa�da e o intervalo de execu��o.
2. Execute o programa.
3. O programa ir� verificar periodicamente o diret�rio de entrada em busca de novos arquivos .zip.
4. Quando um arquivo .zip compat�vel for encontrado, ele ser� extra�do para o diret�rio tempor�rio.
5. Os arquivos .txt presentes dentro do arquivo .zip ser�o movidos para o diret�rio de sa�da especificado.
6. O arquivo .zip original ser� movido para a pasta "extra�dos" dentro do diret�rio de sa�da.

Certifique-se de ter a vers�o correta do .NET instalada e execute o programa usando o comando `dotnet run` no terminal.