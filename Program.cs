using Microsoft.Extensions.Configuration;

namespace ExtratorZipZpl
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FileService fileService = new FileService();

            while (true)
            {
                IEnumerable<string> zipFiles = fileService.GetZipFilesToExtract();

                foreach (string zipFile in zipFiles)
                {
                    await fileService.ExtractAndMoveFilesAsync(zipFile);
                }

                await Task.Delay(1000); // Aguarda 1 segundo antes de verificar novamente
            }
        }
    }
}
