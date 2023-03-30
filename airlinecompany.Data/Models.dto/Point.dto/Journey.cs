using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Models.dto.Point.dto
{
    public class Journey
    {
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
