using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Flights.dto;
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
        public int Add(Flight entity)
        {
            int addResult = _flightRepository.Add(entity);
            return addResult;
        }
        public bool Delete(int id)
        {
            Func<Flight, bool> filter = filter => filter.Id == id;
            bool deleteResult = _flightRepository.Delete(filter);
            return deleteResult;
        }
        public List<Flight>? Get(string name)
        {
            Func<Flight, bool> filter = filter => filter.FlightNumber == name;
            var list = _flightRepository.Get(filter);
            return list;
        }

        public Flight? GetSingle(int id)
        {
            Flight? getSingleResult = _flightRepository.GetSingle(id);
            return getSingleResult;
        }

        public async Task<Flight?> UpdateAsync(int id, Flight updatedEntity)
        {
            Func<Flight, bool> filter = filter => filter.Id == id;
            Flight? updateResult = await _flightRepository.UpdateAsync(filter, updatedEntity);
            return updateResult;
        }

        public async Task<bool> CheckForeignKey(FlightDto flightDto)
        {
            bool isExist = await _flightRepository.CheckAllForeignKeysExistAsync(flightDto);
            return isExist;
        }
        public bool CheckAvailabality(int flightId)
        {
            bool isBusy = _flightRepository.CheckAvailabality(flightId);
            return isBusy;
        }
       
    }
}
