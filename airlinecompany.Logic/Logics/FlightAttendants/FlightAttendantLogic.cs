using airlinecompany.Data.Repositories.FlightAttendants;
using airlinecompany.Logic.Logics.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.FlightAttendants
{
    public class FlightAttendantLogic : IFlightAttendantLogic
    {
        private IFlightAttendantRepository _flightAttendantRepository;
        public FlightAttendantLogic(IFlightAttendantRepository flightAttendantRepository)
        {
            _flightAttendantRepository = flightAttendantRepository;
        }
    }
}
