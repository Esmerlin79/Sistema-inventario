using Sistema.Datos.Repository;
using Sistema.Datos.Repository.Interface;
using Sistema.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Datos.Services
{
    public class CategoriaService : ICategoria
    {
        private readonly IGenericRepository<categoria> _context;
        public CategoriaService(IGenericRepository<categoria> context)
        {
            _context = context;
        }
        public async Task<List<categoria>> getCategoria()
        {
            return await _context.GetAll();
        }
    }
}
