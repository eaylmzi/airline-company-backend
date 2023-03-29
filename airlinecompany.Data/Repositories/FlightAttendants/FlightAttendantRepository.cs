using airlinecompany.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.FlightAttendants
{
    public class FlightAttendantRepository :RepositoryBase<FlightAttendant> ,IFlightAttendantRepository
    {
        AirlineCompanyDBContext _context = new AirlineCompanyDBContext();

        private DbSet<FlightAttendant> query { get; set; }
        public FlightAttendantRepository()
        {
            query = _context.Set<FlightAttendant>();
        }
        public bool CheckAvailabality(int flightAttendantId)
        {
            bool isBusy = _context.Set<Flight>().Any(p => p.FlightAttendantId == flightAttendantId);
            if (isBusy)
            {
                return true;
            }
            return false;
        }
    }
}
