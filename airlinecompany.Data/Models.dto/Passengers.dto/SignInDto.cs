using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Passengers.dto
{
    public class SignInDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
