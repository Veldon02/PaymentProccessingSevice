using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public record Payment(
        string FullName,
        Address Address,
        decimal PaymentAmount,
        DateTime Date,
        long AccountNumber,
        string Service);
}
