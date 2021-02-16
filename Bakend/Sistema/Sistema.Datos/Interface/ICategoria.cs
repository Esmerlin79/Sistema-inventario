using Sistema.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Datos.Repository.Interface
{
    public interface ICategoria 
    {
        Task<List<categoria>> getCategoria();
    }
}
