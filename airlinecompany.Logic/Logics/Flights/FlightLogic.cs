using airlinecompany.Data.Repositories.FlightAttendants;
using airlinecompany.Data.Repositories.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Flights
{
    public class FlightLogic : IFlightLogic
    {
        private IFlightRepository _flightRepository;
        public FlightLogic(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }
    }
}
