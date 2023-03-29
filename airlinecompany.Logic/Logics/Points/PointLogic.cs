using airlinecompany.Data.Models;
using airlinecompany.Data.Repositories.Planes;
using airlinecompany.Data.Repositories.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Points
{
    public class PointLogic : IPointLogic
    {
        private IPointRepository _pointRepository;
        public PointLogic(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }
        public int Add(Point entity)
        {
            int addResult = _pointRepository.Add(entity);
            return addResult;
        }
        public bool Delete(int id)
        {
            Func<Point, bool> filter = filter => filter.Id == id;
            bool deleteResult = _pointRepository.Delete(filter);
            return deleteResult;
        }
        public List<Point>? Get(string name)
        {
            Func<Point, bool> filter = filter => filter.Name == name;
            var list = _pointRepository.Get(filter);
            return list;
        }

        public Point? GetSingle(int id)
        {
            Point? getSingleResult = _pointRepository.GetSingle(id);
            return getSingleResult;
        }
        public Point? GetSingleByName(string name)
        {
            Func<Point, bool> getEntity = getEntity => getEntity.Name == name;
            Point? entity = _pointRepository.GetSingleByMethod(getEntity);
            return entity;
        }

        public async Task<Point>? UpdateAsync(int id, Point updatedEntity)
        {
            Func<Point, bool> filter = filter => filter.Id == id;
            Point? updateResult = await _pointRepository.UpdateAsync(filter, updatedEntity);
            return updateResult;
        }
        public bool CheckAvailabality(int pointId)
        {
            bool isBusy = _pointRepository.CheckAvailabality(pointId);
            return isBusy;
        }
    }
}
