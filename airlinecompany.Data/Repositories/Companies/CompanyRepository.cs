using airlinecompany.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.Companies
{
    public class CompanyRepository : RepositoryBase<Company> , ICompanyRepository
    {

        AirlineCompanyDBContext _context = new AirlineCompanyDBContext();

        private DbSet<Company> query { get; set; }
        public CompanyRepository()
        {
            query = _context.Set<Company>();
        }
        public bool CheckAvailabality(int companyId)
        {
            bool isBusy = _context.Set<Flight>().Any(p => p.CompanyId == companyId) ||
                          _context.Set<FlightAttendant>().Any(p => p.CompanyId == companyId);   
            if (isBusy)
            {
                return true;
            }
            return false;
        }
    }
}
