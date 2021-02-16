using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Datos.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public readonly DbContextSistema _context;
        public GenericRepository(DbContextSistema context)
        {
            _context = context;        
        }
        public async Task<List<TEntity>> GetAll()
        {
            return await  _context.Set<TEntity>().ToListAsync();
        }

        public int Count()
        {
            return _context.Set<TEntity>().ToList().Count();
        }
    }
}
