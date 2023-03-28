using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Flights.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.Flights
{
    public interface IFlightRepository : IRepositoryBase<Flight>
    {
        public Task<bool> CheckAllForeignKeysExistAsync(FlightDto flightDto);
    }
}
