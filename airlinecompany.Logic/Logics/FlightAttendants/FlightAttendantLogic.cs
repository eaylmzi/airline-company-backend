using airlinecompany.Data.Models;
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
        public int Add(FlightAttendant entity)
        {
            int addResult = _flightAttendantRepository.Add(entity);
            return addResult;
        }
        public bool Delete(int id)
        {
            Func<FlightAttendant, bool> filter = filter => filter.Id == id;
            bool deleteResult = _flightAttendantRepository.Delete(filter);
            return deleteResult;
        }
        public List<FlightAttendant>? Get(string name)
        {
            Func<FlightAttendant, bool> filter = filter => filter.Name == name;
            var list = _flightAttendantRepository.Get(filter);
            return list;
        }

        public FlightAttendant? GetSingle(int id)
        {
            FlightAttendant? getSingleResult = _flightAttendantRepository.GetSingle(id);
            return getSingleResult;
        }

        public async Task<FlightAttendant?> UpdateAsync(int id, FlightAttendant updatedEntity)
        {
            Func<FlightAttendant, bool> filter = filter => filter.Id == id;
            FlightAttendant? updateResult = await _flightAttendantRepository.UpdateAsync(filter, updatedEntity);
            return updateResult;
        }
        public bool CheckAvailabality(int flightAttendantId)
        {
            bool isBusy = _flightAttendantRepository.CheckAvailabality(flightAttendantId);
            return isBusy;
        }
    }
}
