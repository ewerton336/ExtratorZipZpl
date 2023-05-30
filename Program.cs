using System.IO.Compression;
using Microsoft.Extensions.Configuration;



// Carrega as configurações do arquivo appsettings.json
IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

// Obtém o caminho do diretório a ser consultado
string directoryIn = configuration.GetSection("DirectoryIn").Value;

if (string.IsNullOrEmpty(directoryIn))
{
    Console.WriteLine("Diretório de entrada inválido. Verifique as configurações em appsettings.json.");
    return;
}

// Cria o diretório "extraídos" dentro do diretório de saída
string directoryOut = configuration.GetSection("DirectoryOut").Value;
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
        // Extrai o arquivo para o diretório de saída
        ZipFile.ExtractToDirectory(file, extractedFolderPath);
        Console.WriteLine("Extração concluída com sucesso.");

        // Move o arquivo extraído para a pasta "extraídos"
        string extractedFileName = Path.GetFileName(file);
        string extractedFilePath = Path.Combine(extractedFolderPath, extractedFileName);
        string destinationFilePath = GetUniqueFilePath(extractedFolderPath, extractedFileName);
        File.Move(extractedFilePath, destinationFilePath);
        Console.WriteLine($"Arquivo movido para a pasta 'extraídos' com o nome '{Path.GetFileName(destinationFilePath)}'.");

        // Move o arquivo original para a pasta "extraídos"
        string originalDestinationFilePath = Path.Combine(extractedFolderPath, extractedFileName);
        File.Move(file, originalDestinationFilePath);
        Console.WriteLine($"Arquivo original movido para a pasta 'extraídos' com o nome '{Path.GetFileName(originalDestinationFilePath)}'.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao extrair arquivo: {ex.Message}");
    }
}

Console.WriteLine("Pressione qualquer tecla para sair.");
Console.ReadKey();

string GetUniqueFilePath(string directory, string fileName)
{
    string filePath = Path.Combine(directory, fileName);
    string fileExtension = Path.GetExtension(fileName);
    string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
    int counter = 1;

    while (File.Exists(filePath))
    {
        string newFileName = $"{fileNameOnly} ({counter}){fileExtension}";
        filePath = Path.Combine(directory, newFileName);
        counter++;
    }

    return filePath;
}
