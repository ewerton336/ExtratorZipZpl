using ExtratorZipZpl;

namespace FileExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fileService = new FileService();
            while (true)
            {
                try
                {
                    Console.WriteLine("Aguardando novos arquivos .zip...");

                    while (true)
                    {
                        try
                        {
                            IEnumerable<string> files = fileService.GetZipFilesToExtract();

                            foreach (string file in files)
                            {
                                Console.WriteLine($"Extraindo arquivo: {file}");
                                await fileService.ExtractAndMoveFilesAsync(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao extrair arquivo: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao extrair arquivo: {ex.Message}");
                }
            }
        }
    }
}

