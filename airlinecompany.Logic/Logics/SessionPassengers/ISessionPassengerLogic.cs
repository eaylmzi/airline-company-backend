using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.SessionPassengers
{
    public interface ISessionPassengerLogic 
    {
        public int Add(SessionPassenger entity);
        public bool Delete(int id);
        public List<SessionPassenger>? GetByPassengerId(int id);
        public SessionPassenger? GetSingle(int id);
        public Task<SessionPassenger?> UpdateAsync(int id, SessionPassenger updatedEntity);
    }
}
