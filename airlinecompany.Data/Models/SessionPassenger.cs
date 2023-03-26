using System;
using System.Collections.Generic;

namespace airlinecompany.Data.Models
{
    public partial class SessionPassenger
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int FlightId { get; set; }
    }
}
