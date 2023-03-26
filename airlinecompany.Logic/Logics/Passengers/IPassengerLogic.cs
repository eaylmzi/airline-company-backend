using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Passengers
{
    public interface IPassengerLogic
    {
        public int Add(Passenger entity);
        public bool Delete(int id);
        public List<Passenger>? Get(string name);
        public Passenger? GetSingle(int id);
        public Passenger? GetSingleByUsername(string username);
        public Task<Passenger>? UpdateAsync(int id, Passenger updatedEntity);
    }
}
