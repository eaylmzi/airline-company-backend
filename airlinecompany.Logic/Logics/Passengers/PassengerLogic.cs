using airlinecompany.Data.Repositories.Flights;
using airlinecompany.Data.Repositories.Passengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Passengers
{
    public class PassengerLogic : IPassengerLogic
    {
        private IPassengerRepository _passengerRepository;
        public PassengerLogic(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }
    }
}
