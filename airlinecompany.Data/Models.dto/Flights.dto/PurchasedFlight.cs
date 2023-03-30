using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Flights.dto
{
    public class PurchasedFlight
    {
        public string CompanyName { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string BeginningName { get; set; } = null!;
        public string FinalName { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
