using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.Planes
{
    public interface IPlaneRepository :  IRepositoryBase<Plane>
    {
        public bool CheckAvailabality(int planeId);
    }
}
