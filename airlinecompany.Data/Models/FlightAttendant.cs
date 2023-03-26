using System;
using System.Collections.Generic;

namespace airlinecompany.Data.Models
{
    public partial class FlightAttendant
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
