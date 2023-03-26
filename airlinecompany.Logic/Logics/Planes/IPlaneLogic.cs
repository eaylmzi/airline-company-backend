using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Planes
{
    public interface IPlaneLogic
    {
        public int Add(Plane entity);
        public bool Delete(int id);
        public List<Plane>? Get(string name);
        public Plane? GetSingle(int id);
        public Task<Plane>? UpdateAsync(int id, Plane updatedEntity);
    }
}
