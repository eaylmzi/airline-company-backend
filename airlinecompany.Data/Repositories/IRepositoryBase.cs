using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        public int Add(T entity);
        public bool Delete(Func<T, bool> method);
        public Task<T>? UpdateAsync(Func<T, bool> method, T? updatedEntity);       
        public List<T>? Get(Func<T, bool> method);
        public T? GetSingle(int number);
        public T? GetSingleByMethod(Func<T, bool> method);


    }
}
