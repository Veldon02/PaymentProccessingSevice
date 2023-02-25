using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Application.Services;

namespace PaymentProccessingService
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input = ConfigurationManager.AppSettings["inputPath"];
            var output = ConfigurationManager.AppSettings["outputPath"];

            if (input == null || output == null)
            {
                Console.WriteLine("App.config file missing or is not correct. Please check it");
                return;
            }

            var paymentProccessing = new PaymentStatisticService(output);
            paymentProccessing.ProccessDirectory(input!);

            ConfigureFileSystemWatcher(input, output, paymentProccessing);
            ConfigureMidnightNotifier(paymentProccessing);

            Console.WriteLine("Application is runnig");
            Console.WriteLine($"Source path: {input}");
            Console.WriteLine($"Destination path: {output}");

            Console.WriteLine("\n\nType stop to stop the application");
            while (Console.ReadLine() != "stop") { }

            Console.WriteLine("Application stoped");
        }

        private static void ConfigureMidnightNotifier(PaymentStatisticService paymentProccessing)
        {
            var midnightNotifier = new MidnightNotifier();
            midnightNotifier.Timer.Elapsed += (sender, e) => { paymentProccessing.Log(); };
        }

        private static void ConfigureFileSystemWatcher(string input, string output, PaymentStatisticService paymentProccessing)
        {
            var wacher = new FileSystemWatcher(input!);

            wacher.Created += (sender, e) => paymentProccessing.ProccessFile(e.FullPath);
            wacher.EnableRaisingEvents = true;
        }
    }
}