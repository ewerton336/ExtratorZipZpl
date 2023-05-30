using Microsoft.Extensions.Configuration;
using System.IO.Compression;

namespace ExtratorZipZpl
{
    class FileService
    {
        public readonly string directoryIn;
        public readonly string directoryOut;
        public readonly string extractedFolderPath;
        public readonly string directoryOutTxtEtiqueta;
        public readonly int intervaloExecucao;

        public FileService()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            directoryIn = configuration.GetSection("DirectoryZipIn").Value;
            directoryOut = configuration.GetSection("DirectoryZipOut").Value;
            intervaloExecucao = configuration.GetValue<int>("IntervaloExecucao");
            directoryOutTxtEtiqueta = configuration.GetSection("DirectoryTxtEtiqueta").Value;
            extractedFolderPath = Path.Combine(directoryOut, "Extraídos");
            Directory.CreateDirectory(extractedFolderPath);
        }

        public IEnumerable<string> GetZipFilesToExtract()
        {
            return Directory.EnumerateFiles(directoryIn, "*.zip", SearchOption.TopDirectoryOnly)
                .Where(file => Path.GetFileName(file).IndexOf("ZPL", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public async Task ExtractAndMoveFilesAsync(string zipFilePath)
        {
            string tempFolderPath = Path.Combine(directoryOut, "temp");
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }
            ClearDirectory(tempFolderPath);

            ZipFile.ExtractToDirectory(zipFilePath, tempFolderPath);
            Console.WriteLine("Extração concluída com sucesso.");

            string[] txtFiles = Directory.GetFiles(tempFolderPath, "*.txt", SearchOption.AllDirectories);

            foreach (string txtFile in txtFiles)
            {
                string relativePath = Path.GetRelativePath(tempFolderPath, txtFile);
                string destinationFilePath = Path.Combine(directoryOutTxtEtiqueta, relativePath);

                if (File.Exists(destinationFilePath))
                {
                    string newFilePath = GetUniqueFilePath(destinationFilePath);
                    await MoveFileAsync(txtFile, newFilePath);
                    Console.WriteLine($"Arquivo movido para o diretório de saída com o nome '{Path.GetFileName(newFilePath)}'.");
                }
                else
                {
                    await MoveFileAsync(txtFile, destinationFilePath);
                    Console.WriteLine($"Arquivo movido para o diretório de saída com o nome '{Path.GetFileName(destinationFilePath)}'.");
                }
            }

            string zipDestinationFilePath = Path.Combine(extractedFolderPath, Path.GetFileName(zipFilePath));
            MoveFile(zipFilePath, zipDestinationFilePath);
            Console.WriteLine($"Arquivo ZIP movido para a pasta 'extraídos' com o nome '{Path.GetFileName(zipDestinationFilePath)}'.");

            ClearDirectory(tempFolderPath);

            Task.Delay(intervaloExecucao).Wait();
        }

        public async Task MoveFileAsync(string sourceFilePath, string destinationFilePath)
        {
            await Task.Run(() => File.Move(sourceFilePath, destinationFilePath));
        }

        public void MoveFile(string sourceFilePath, string destinationFilePath)
        {
            File.Move(sourceFilePath, destinationFilePath);
        }

        public string GetUniqueFilePath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string fileExtension = Path.GetExtension(filePath);
            string newFileName = GetUniqueFileName(fileName);
            return Path.Combine(directory, newFileName + fileExtension);
        }

        public string GetUniqueFileName(string fileName)
        {
            string newFileName = fileName;
            int counter = 2;

            while (File.Exists(newFileName))
            {
                newFileName = $"{fileName} ({counter})";
                counter++;
            }

            return newFileName;
        }

        public void ClearDirectory(string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true);
            }
        }
    }
}
