﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Passengers.dto
{
    public class PassengerDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string? Token { get; set; }
        public int? Money { get; set; }
    }
}
