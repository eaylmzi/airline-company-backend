using System;
using System.Collections.Generic;

namespace airlinecompany.Data.Models
{
    public partial class Flight
    {
        public int Id { get; set; }
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
