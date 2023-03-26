using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.FlightAttendant.dto
{
    public class FlightAttendantDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
