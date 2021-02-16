using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Usuarios;
using Sistema.Web.Models.Usuarios.Rol;

namespace Sistema.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public RolesController(DbContextSistema context)
        {
            _context = context;
        }


        // GET: api/ /Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolViewModel>> Listar()
        {
            var rol = await _context.rol.ToListAsync();

            return rol.Select(x => new RolViewModel
            {
                idrol = x.idrol,
                nombre = x.nombre,
                descripcion = x.descripcion,
                condicion = x.condicion
            });

        }

        // GET: api/Roles/Select
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> Select()
        {
            var rol = await _context.rol.Where(x => x.condicion == true).ToListAsync();

            return rol.Select(x => new SelectViewModel
            {
                idrol = x.idrol,
                nombre = x.nombre,

            });

        }
        private bool rolExists(int id)
        {
            return _context.rol.Any(e => e.idrol == id);
        }
    }
}