using airlinecompany.Data.Models;
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
        public int Add(Passenger entity)
        {
            int addResult = _passengerRepository.Add(entity);
            return addResult;
        }
        public bool Delete(int id)
        {
            Func<Passenger, bool> filter = filter => filter.Id == id;
            bool deleteResult = _passengerRepository.Delete(filter);
            return deleteResult;
        }
        public List<Passenger>? Get(string name)
        {
            Func<Passenger, bool> filter = filter => filter.UserName == name;
            var list = _passengerRepository.Get(filter);
            return list;
        }

        public Passenger? GetSingle(int id)
        {
            Passenger? getSingleResult = _passengerRepository.GetSingle(id);
            return getSingleResult;
        }
        public Passenger? GetSingleByUsername(string username)
        {
            Func<Passenger, bool> getEntity = getEntity => getEntity.UserName == username;
            Passenger? entity = _passengerRepository.GetSingleByMethod(getEntity);
            return entity;
        }
        public Passenger? GetSingleByUsernameAndPassword(string username,string password)
        {
            Func<Passenger, bool> getUserName = getUserName => getUserName.UserName == username;
            Func<Passenger, bool> getPassword = getPassword => getPassword.Password == password;
            Passenger? entity = _passengerRepository.GetSingleByMethod(getUserName, getPassword);
            return entity;
        }

        public async Task<Passenger>? UpdateAsync(int id, Passenger updatedEntity)
        {
            Func<Passenger, bool> filter = filter => filter.Id == id;
            Passenger? updateResult = await _passengerRepository.UpdateAsync(filter, updatedEntity);
            return updateResult;
        }
    }
}
