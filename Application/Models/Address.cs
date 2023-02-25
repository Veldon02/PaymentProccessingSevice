using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Address
    {
        public Address(string city, string fullAddress)
        {
            City = city;
            FullAddress = fullAddress;
        }

        public string City { get; set; }
        public string FullAddress { get; set; }
    }
}
