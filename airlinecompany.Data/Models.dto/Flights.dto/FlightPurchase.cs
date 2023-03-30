using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Flights.dto
{
    public class FlightPurchase
    {
        public int FlightId { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public string? PassengerUserName { get; set; }

    }
    //using date, from, to, passenger name 
}
