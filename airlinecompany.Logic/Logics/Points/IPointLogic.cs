using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Points
{
    public interface IPointLogic
    {
        public int Add(Point entity);
        public bool Delete(int id);
        public List<Point>? Get(string name);
        public Point? GetSingle(int id);
        public Point? GetSingleByName(string name);
        public Task<Point>? UpdateAsync(int id, Point updatedEntity);
        public bool CheckAvailabality(int pointId);
    }
}
