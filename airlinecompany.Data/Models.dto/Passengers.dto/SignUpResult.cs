using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Passengers.dto
{
    public class SignUpResult
    {
        public string ResultMessage { get; set; } = null!;
        public int isPassengerAdded { get; set; }
        public Passenger? passenger { get; set; }

    }
}
