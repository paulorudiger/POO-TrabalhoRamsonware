// See https://aka.ms/new-console-template for more information
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Informe o caminho da pasta que deseja compactar:");
        string pastaOrigem = Console.ReadLine();

        if (!Directory.Exists(pastaOrigem))
        {
            Console.WriteLine("A pasta informada não existe.");
            return;
        }

        Console.WriteLine("Informe o caminho do arquivo ZIP de destino:");
        string arquivoDestino = Console.ReadLine();

        Console.WriteLine("Informe a senha para o arquivo ZIP:");
        string senha = Console.ReadLine();

        // Compactar a pasta
        CompactarPastaComSenha(pastaOrigem, arquivoDestino, senha);

        // Apagar a pasta original e enviá-la para a lixeira
        ApagarPasta(pastaOrigem);

        Console.WriteLine("Processo concluído.");
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

    static void ApagarPasta(string pastaOrigem)
    {
        try
        {
            // Apaga a pasta enviando para a lixeira
            FileSystem.DeleteDirectory(pastaOrigem, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            Console.WriteLine("Pasta original apagada e enviada para a lixeira.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao apagar a pasta: {ex.Message}");
        }
    }
}