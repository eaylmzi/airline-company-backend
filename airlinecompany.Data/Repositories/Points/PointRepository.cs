using airlinecompany.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.Points
{
    public class PointRepository : RepositoryBase<Point>,IPointRepository
    {
        AirlineCompanyDBContext _context = new AirlineCompanyDBContext();

        private DbSet<Point> query { get; set; }
        public PointRepository()
        {
            query = _context.Set<Point>();
        }
        public bool CheckAvailabality(int companyId)
        {
            bool isBusy = _context.Set<Flight>().Any(p => p.StartPoint == companyId) ||
                          _context.Set<Flight>().Any(p => p.FinalPoint == companyId);
            if (isBusy)
            {
                return true;
            }
            return false;
        }

    }
}
