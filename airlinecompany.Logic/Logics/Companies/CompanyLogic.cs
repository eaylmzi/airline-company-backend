using airlinecompany.Data.Models;
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
        public int Add(Company entity)
        {
            int addResult = _companyRepository.Add(entity);
            return addResult;
        }
        public bool Delete(int id)
        {
            Func<Company, bool> filter = filter => filter.Id == id;
            bool deleteResult = _companyRepository.Delete(filter);
            return deleteResult;
        }
        public List<Company>? Get(string name)
        {
            Func<Company, bool> filter = filter => filter.Name == name;
            var list = _companyRepository.Get(filter);
            return list;
        }

        public Company? GetSingle(int id)
        {
            Company? getSingleResult = _companyRepository.GetSingle(id);
            return getSingleResult;
        }

        public async Task<Company>? UpdateAsync(int id, Company updatedEntity)
        {
            Func<Company, bool> filter = filter => filter.Id == id;
            Company? updateResult = await _companyRepository.UpdateAsync(filter, updatedEntity);
            return updateResult;
        }
        public bool CheckAvailabality(int companyId)
        {
            bool isBusy = _companyRepository.CheckAvailabality(companyId);
            return isBusy;
        }

    }
}
