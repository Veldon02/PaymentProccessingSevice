using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class Parser
    {
        public Payment? ParseLine(string line)
        {
            List<string> fields = new List<string>();

            bool inQuotes = false;
            int fieldStartIndex = 0;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '“')
                    inQuotes = true;
                else if (c == '”')
                    inQuotes = false;
                else if (c == '"')
                    inQuotes = !inQuotes;
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(line.Substring(fieldStartIndex, i - fieldStartIndex));
                    fieldStartIndex = i + 1;
                }
            }

            fields.Add(line.Substring(fieldStartIndex));
            return LineToPayment(fields);
        }
        private Payment? LineToPayment(List<string> fields)
        {
            try
            {
                foreach (var field in fields)
                {
                    if (string.IsNullOrEmpty(field)) throw new InvalidDataException();
                }
                var fullName = fields[0] + " " + fields[1];
                var fullAddress = fields[2].Substring(1, fields[2].Length - 3);
                var address = new Address(fullAddress.Split(",")[0], fullAddress);
                var paymentAmount = decimal.Parse(fields[3]);
                var dateArr = fields[4].Split("-")
                        .Select(x => int.Parse(x))
                        .ToArray();
                var date = new DateTime(dateArr[0], dateArr[2], dateArr[1]);
                var accountNumber = long.Parse(fields[5]);

                var payment = new Payment(
                    fullName,
                    address,
                    paymentAmount,
                    date,
                    accountNumber,
                    fields[6]);
                return payment;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
