using airlinecompany.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Logic.Logics.Companies
{
    public interface ICompanyLogic
    {
        public int Add(Company entity);
        public bool Delete(int id);
        public List<Company>? Get(string name);
        public Company? GetSingle(int id);
        public Task<Company>? UpdateAsync(int id, Company updatedEntity);
        public bool CheckAvailabality(int companyId);
    }
}
