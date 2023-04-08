using airlinecompany.Data.Models;
using airlinecompany.Data.Repositories.Points;
using airlinecompany.Data.Repositories.SessionPassengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.SessionPassengers
{
    public class SessionPassengerLogic : ISessionPassengerLogic
    {
        private ISessionPassengerRepository _sessionPassengerRepository;
        public SessionPassengerLogic(ISessionPassengerRepository sessionPassengerRepository)
        {
            _sessionPassengerRepository = sessionPassengerRepository;
        }
        public int Add(SessionPassenger entity)
        {
            int addResult = _sessionPassengerRepository.Add(entity);
            return addResult;
        }
        public bool Delete(int id)
        {
            Func<SessionPassenger, bool> filter = filter => filter.Id == id;
            bool deleteResult = _sessionPassengerRepository.Delete(filter);
            return deleteResult;
        }
        public List<SessionPassenger>? GetByPassengerId(int id)
        {
            Func<SessionPassenger, bool> filter = filter => filter.PassengerId == id;
            var list = _sessionPassengerRepository.Get(filter);
            return list;
        }

        public SessionPassenger? GetSingle(int id)
        {
            SessionPassenger? getSingleResult = _sessionPassengerRepository.GetSingle(id);
            return getSingleResult;
        }

        public async Task<SessionPassenger?> UpdateAsync(int id, SessionPassenger updatedEntity)
        {
            Func<SessionPassenger, bool> filter = filter => filter.Id == id;
            SessionPassenger? updateResult = await _sessionPassengerRepository.UpdateAsync(filter, updatedEntity);
            return updateResult;
        }

    }
}
