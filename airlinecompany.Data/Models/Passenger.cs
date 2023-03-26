using System;
using System.Collections.Generic;

namespace airlinecompany.Data.Models
{
    public partial class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Token { get; set; } = null!;
        public int? Money { get; set; }
    }
}
