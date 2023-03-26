using airlinecompany.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airlinecompany.Data.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        AirlineCompanyDBContext _context = new AirlineCompanyDBContext();

        private DbSet<T> query { get; set; }
        public RepositoryBase()
        {
            query = _context.Set<T>();
        }
        public int Add(T entity)
        {
            if (entity != null)
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                var id = _context.Entry(entity).Property("Id").CurrentValue;
                return (int)id;
            }
            else
            {
                return -1;
            }
        }

        public bool Delete(Func<T, bool> method)
        {
            T? entity = query
                     .Where(method)
                     .Select(m => m)
                     .SingleOrDefault();
            if (entity != null)
            {
                query.Remove(entity);
                _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public List<T> Get(Func<T, bool> method)
        {
            var list = query
                      .Where(method)
                      .Select(m => m)
                      .ToList();
            return list;
        }

        public T? GetSingle(int number)
        {
            return query.Find(number);
        }
        public T? GetSingleByMethod(Func<T, bool> method)
        {
            T? entity = query
                      .Where(method)
                      .Select(m => m)
                      .SingleOrDefault();


            return entity;
        }

        public async Task<T>? UpdateAsync(Func<T, bool> metot, T? updatedEntity)
        {
            T? entity = query
                     .Where(metot)
                     .Select(m => m)
                     .SingleOrDefault();

            if (entity != null)
            {            
                _context.Entry(entity).CurrentValues.SetValues(updatedEntity);
                await _context.SaveChangesAsync();
                return updatedEntity;
            }

            return null;
        }
    }
}




