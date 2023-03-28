using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Flights.dto
{
    public class FlightDto
    {
        public int CompanyId { get; set; }
        public int PlaneNumber { get; set; }
        public string FlightNumber { get; set; } = null!;
        public int Price { get; set; }
        public int StartPoint { get; set; }
        public int FinalPoint { get; set; }
        public DateTime Date { get; set; }
        public int FlightAttendantId { get; set; }
    }
}
