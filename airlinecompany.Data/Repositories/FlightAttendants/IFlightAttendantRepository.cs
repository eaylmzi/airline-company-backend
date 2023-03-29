using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.FlightAttendants
{
    public interface IFlightAttendantRepository : IRepositoryBase<FlightAttendant>
    {
        public bool CheckAvailabality(int flightAttendantId);
    }
}
