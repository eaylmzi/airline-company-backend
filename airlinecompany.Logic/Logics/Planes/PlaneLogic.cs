using airlinecompany.Data.Models;
using airlinecompany.Data.Repositories.Passengers;
using airlinecompany.Data.Repositories.Planes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Planes
{
    public class PlaneLogic : IPlaneLogic
    {
        private IPlaneRepository _planeRepository;
        public PlaneLogic(IPlaneRepository planeRepository)
        {
            _planeRepository = planeRepository;
        }
        public int Add(Plane entity)
        {
            int addResult = _planeRepository.Add(entity);
            return addResult;
        }
        public bool Delete(int id)
        {
            Func<Plane, bool> filter = filter => filter.Id == id;
            bool deleteResult = _planeRepository.Delete(filter);
            return deleteResult;
        }
        public List<Plane>? Get(string name)
        {
            Func<Plane, bool> filter = filter => filter.Model == name;
            var list = _planeRepository.Get(filter);
            return list;
        }

        public Plane? GetSingle(int id)
        {
            Plane? getSingleResult = _planeRepository.GetSingle(id);
            return getSingleResult;
        }

        public async Task<Plane>? UpdateAsync(int id, Plane updatedEntity)
        {
            Func<Plane, bool> filter = filter => filter.Id == id;
            Plane? updateResult = await _planeRepository.UpdateAsync(filter, updatedEntity);
            return updateResult;
        }
    }
}
