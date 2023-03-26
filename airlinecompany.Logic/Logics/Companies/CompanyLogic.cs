using airlinecompany.Data.Repositories.Companies;
using airlinecompany.Data.Repositories.Passengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Companies
{
    public class CompanyLogic : ICompanyLogic
    {
        private ICompanyRepository _companyRepository;
        public CompanyLogic(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
    }
}
