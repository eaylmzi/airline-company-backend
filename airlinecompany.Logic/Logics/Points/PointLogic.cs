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
    }
}
