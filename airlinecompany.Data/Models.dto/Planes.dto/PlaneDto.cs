using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Planes.dto
{
    public class PlaneDto
    {
        public string Model { get; set; } = null!;
        public int SeatNumber { get; set; }
    }
}
