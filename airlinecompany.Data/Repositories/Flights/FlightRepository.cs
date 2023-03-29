using airlinecompany.Data.Models;
using airlinecompany.Data.Models.dto.Flights.dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.Flights
{
    public class FlightRepository : RepositoryBase<Flight> , IFlightRepository
    {
        AirlineCompanyDBContext _context = new AirlineCompanyDBContext();

        private DbSet<Flight> query { get; set; }
        public FlightRepository()
        {
            query = _context.Set<Flight>();
        }
        public async Task<bool> CheckAllForeignKeysExistAsync(FlightDto flightDto)
        {

            return await _context.Set<Company>().AnyAsync(c => c.Id == flightDto.CompanyId)
            && await _context.Set<Plane>().AnyAsync(p => p.Id == flightDto.PlaneNumber)
                && await _context.Set<Point>().AnyAsync(sp => sp.Id == flightDto.StartPoint)
                && await _context.Set<Point>().AnyAsync(fp => fp.Id == flightDto.FinalPoint)
                && await _context.Set<FlightAttendant>().AnyAsync(fp => fp.Id == flightDto.FinalPoint)
                && await _context.Set<FlightAttendant>().AnyAsync(fp => fp.CompanyId == flightDto.CompanyId);
        }
        public bool CheckAvailabality(int flightId)
        {
            bool isBusy = _context.Set<SessionPassenger>().Any(p => p.FlightId == flightId);
            if (isBusy)
            {
                return true;
            }
            return false;
        }
    }
}
