using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace FileExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Carrega as configurações do arquivo appsettings.json
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Obtém o caminho do diretório de entrada e saída
            string directoryIn = configuration.GetSection("DirectoryIn").Value;
            string directoryOut = configuration.GetSection("DirectoryOut").Value;

            if (string.IsNullOrEmpty(directoryIn) || string.IsNullOrEmpty(directoryOut))
            {
                Console.WriteLine("Diretórios inválidos. Verifique as configurações em appsettings.json.");
                return;
            }

            // Cria o diretório "extraídos" dentro do diretório de saída
            string extractedFolderPath = Path.Combine(directoryOut, "extraídos");
            Directory.CreateDirectory(extractedFolderPath);

            // Obtém todos os arquivos no diretório de entrada com extensão .zip e contendo a palavra "ZPL" no nome
            IEnumerable<string> files = Directory.EnumerateFiles(directoryIn, "*.zip", SearchOption.AllDirectories)
                .Where(file => Path.GetFileName(file).IndexOf("ZPL", StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (string file in files)
            {
                Console.WriteLine($"Extraindo arquivo: {file}");

                try
                {
                    // Extrai o arquivo para o diretório temporário
                    string tempFolderPath = Path.Combine(directoryOut, "temp");
                    ZipFile.ExtractToDirectory(file, tempFolderPath);
                    Console.WriteLine("Extração concluída com sucesso.");

                    // Obtém todos os arquivos .txt dentro do diretório temporário
                    string[] txtFiles = Directory.GetFiles(tempFolderPath, "*.txt", SearchOption.AllDirectories);

                    foreach (string txtFile in txtFiles)
                    {
                        string relativePath = Path.GetRelativePath(tempFolderPath, txtFile);
                        string destinationFilePath = Path.Combine(directoryOut, relativePath);

                        // Verifica se o arquivo já existe no diretório de saída
                        if (File.Exists(destinationFilePath))
                        {
                            string fileName = Path.GetFileNameWithoutExtension(destinationFilePath);
                            string fileExtension = Path.GetExtension(destinationFilePath);
                            string newFileName = GetUniqueFileName(fileName, fileExtension);
                            string newFilePath = Path.Combine(directoryOut, newFileName);
                            File.Move(txtFile, newFilePath);
                            Console.WriteLine($"Arquivo movido para o diretório de saída com o nome '{newFileName}'.");
                        }
                        else
                        {
                            File.Move(txtFile, destinationFilePath);
                            Console.WriteLine($"Arquivo movido para o diretório de saída com o nome '{Path.GetFileName(destinationFilePath)}'.");
                        }
                    }

                    // Move o arquivo .zip para a pasta "extraídos"
                    string zipDestinationFilePath = Path.Combine(extractedFolderPath, Path.GetFileName(file));
                    File.Move(file, zipDestinationFilePath);
                    Console.WriteLine($"Arquivo ZIP movido para a pasta 'extraídos' com o nome '{Path.GetFileName(zipDestinationFilePath)}'.");

                    // Exclui o diretório temporário
                    Directory.Delete(tempFolderPath, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao extrair arquivo: {ex.Message}");
                }
            }

            Console.WriteLine("Pressione qualquer tecla para sair.");
            Console.ReadKey();
        }

        static string GetUniqueFileName(string fileName, string fileExtension)
        {
            string newFileName = $"{fileName} ({Guid.NewGuid()}){fileExtension}";
            return newFileName;
        }
    }
}
