using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories.Companies
{
    public interface ICompanyRepository : IRepositoryBase<Company>
    {
        public bool CheckAvailabality(int companyId);
    }
}
