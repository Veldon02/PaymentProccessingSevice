using Application.Models;
using System.IO;
using System.Xml.Linq;

namespace Application.Services
{
    public class PaymentStatisticService
    {
        private readonly FileService _fileService;
        private readonly Parser _parser;
        private int _parsedFileCount = 0;
        private List<string> _invalidFiles = new();
        private long _parsedLineCount = 0;
        private int _errorsCount = 0;
        private int _id = 0;
        private string _outputPath;
        public PaymentStatisticService(string outputPath)
        {
            _fileService = new FileService();
            _parser = new Parser();
            _outputPath = CreateOutputPath(outputPath);
        }

        private string CreateOutputPath(string outputPath)
        {
            var dir = Directory.CreateDirectory(Path.Combine(outputPath, DateTime.Now.ToString("MM-dd-yyyy")));
            return dir.FullName;
        }

        public async Task ProccessDirectory(string input)
        {
            var files = Directory.GetFiles(input);

            foreach (var file in files)
            {
                await ProccessFile(file);
            }
        }

        public async Task ProccessFile(string file)   //make async
        {

            var extension = Path.GetExtension(file);
            if (extension != ".txt" && extension != ".csv") {
                return;
            }

            var payments = new List<Payment>();

            using (StreamReader streamReader = new StreamReader(file))
            {
                if (Path.GetExtension(file) == ".csv")
                    streamReader.ReadLine();

                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var payment = _parser.ParseLine(line!);
                    if (payment == null)
                    {
                        _invalidFiles.Add(file);
                        _parsedFileCount++;
                        _errorsCount++;
                        return;
                    }
                    payments.Add(payment);
                    _parsedLineCount++;
                }
            }

            var cityStatistics = await Task.Run( () => { return ProccessPayments(payments); });  //make async
            _parsedFileCount++;
            _id++;
            _fileService.SerializeObjectIntoFile(cityStatistics, @$"{_outputPath}\output{_id}.json");
        }

        private List<CityStatistic> ProccessPayments(List<Payment> payments)
        {
            var cityStatisctics = new List<CityStatistic>();
            var cities = payments.GroupBy(x => x.Address.City);
            foreach (var c in cities) {

                var services = c
                    .GroupBy(x => x.Service);

                var serviceStatistics = new List<ServiceStatistic>();
                foreach (var service in services)
                {
                    var payers = service
                       .Select(x => new Payer()
                           {
                               Name = x.FullName,
                               Payment = x.PaymentAmount,
                               Date = x.Date,
                               AccountNumber = x.AccountNumber
                           })
                       .ToList();

                    serviceStatistics.Add(new ServiceStatistic()
                        {
                            Name = service.Key,
                            Payers = payers,
                            Total = payers.Sum(x => x.Payment)
                        });
                }

                cityStatisctics.Add(new CityStatistic()
                    {
                        City = c.Key,
                        Services = serviceStatistics,
                        Total = serviceStatistics.Sum(x => x.Total)
                    });
            }

            return cityStatisctics;
        }

        public void Log()
        {
            var logRecord = new LogRecord()
            {
                ParsedFiles = _parsedFileCount,
                ParsedLines = _parsedLineCount,
                FoundErrors = _errorsCount,
                InvalidFiles = _invalidFiles
            };
            _fileService.SerializeObjectIntoFile(logRecord, Path.Combine(_outputPath,"meta.log"));
            Reset();
        }

        private void Reset()
        {
            _parsedFileCount = 0;
            _parsedLineCount = 0;
            _errorsCount = 0;
            _invalidFiles.Clear();
            _id = 0;
        }

    }
}


