using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Flights.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Flights
{
    public interface IFlightLogic
    {
        public int Add(Flight entity);
        public bool Delete(int id);
        public List<Flight>? Get(string name);
        public Flight? GetSingle(int id);
        public Task<Flight>? UpdateAsync(int id, Flight updatedEntity);
        public Task<bool> CheckForeignKey(FlightDto flightDto);
    }
}
