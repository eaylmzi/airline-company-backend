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
    }
}
