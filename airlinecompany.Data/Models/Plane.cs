using System;
using System.Collections.Generic;

namespace AirlineCompanyAPI.Models
{
    public partial class Plane
    {
        public int Id { get; set; }
        public string Model { get; set; } = null!;
        public int SeatNumber { get; set; }
    }
}
