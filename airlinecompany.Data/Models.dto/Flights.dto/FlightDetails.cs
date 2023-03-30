using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Flights.dto
{
    public class FlightDetails
    {
        public int FlightId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public int Price { get; set; }
        public string BeginningName { get; set; } = null!;
        public string FinalName { get; set; } = null!;
        public DateTime Date { get; set; }

    }
}
