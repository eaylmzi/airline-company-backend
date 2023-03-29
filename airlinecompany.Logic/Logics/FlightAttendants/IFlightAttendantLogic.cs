using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.FlightAttendants
{
    public interface IFlightAttendantLogic
    {
        public int Add(FlightAttendant entity);
        public bool Delete(int id);
        public List<FlightAttendant>? Get(string name);
        public FlightAttendant? GetSingle(int id);
        public Task<FlightAttendant>? UpdateAsync(int id, FlightAttendant updatedEntity);
        public bool CheckAvailabality(int flightAttendantId);
    }
}
