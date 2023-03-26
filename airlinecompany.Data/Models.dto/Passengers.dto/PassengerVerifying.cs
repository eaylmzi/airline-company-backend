using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Passengers.dto
{
    public class PassengerVerifying
    { 
        public int Id { get; set; }
        public string Token { get; set; } = null!;
    }
}
