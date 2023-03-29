using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.Points
{
    public interface IPointRepository : IRepositoryBase<Point>
    {
        public bool CheckAvailabality(int companyId);
    }
}
