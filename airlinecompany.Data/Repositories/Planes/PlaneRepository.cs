using airlinecompany.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.Planes
{
    public class PlaneRepository : RepositoryBase<Plane> , IPlaneRepository
    {
        AirlineCompanyDBContext _context = new AirlineCompanyDBContext();

        private DbSet<Plane> query { get; set; }
        public PlaneRepository()
        {
            query = _context.Set<Plane>();
        }
        public bool CheckAvailabality(int planeId)
        {
            bool isBusy = _context.Set<Flight>().Any(p => p.PlaneNumber == planeId);
            if (isBusy)
            {
                return false;
            }
            return true;
        }
    }
}
