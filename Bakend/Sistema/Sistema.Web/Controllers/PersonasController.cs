using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Ventas;
using Sistema.Web.Models.Ventas.Persona;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public PersonasController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Personas/ListarClientes
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonaViewModel>> ListarClientes()
        {
            var persona = await _context.Persona.Where(x => x.tipo_persona == "Cliente").ToListAsync();

            return persona.Select(x => new PersonaViewModel
            {
                idpersona = x.idpersona,
                tipo_persona = x.tipo_persona,
                nombre = x.nombre,
                tipo_documento = x.tipo_documento,
                num_documento = x.num_documento,
                direccion = x.direccion,
                telefono = x.telefono,
                email = x.email
            });

        }

        // GET: api/Personas/ListarProveedores  
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonaViewModel>> ListarProveedores()
        {
            var persona = await _context.Persona.Where(x => x.tipo_persona == "Proveedor").ToListAsync();

            return persona.Select(x => new PersonaViewModel
            {
                idpersona = x.idpersona,
                tipo_persona = x.tipo_persona,
                nombre = x.nombre,
                tipo_documento = x.tipo_documento,
                num_documento = x.num_documento,
                direccion = x.direccion,
                telefono = x.telefono,
                email = x.email
            });

        }


        // GET: api/Personas/SelectProveedores
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> SelectProveedores()
        {
            var persona = await _context.Persona.Where(x => x.tipo_persona == "Proveedor").ToListAsync();

            return persona.Select(x => new SelectViewModel
            {
                idpersona = x.idpersona,
                nombre = x.nombre

            });

        }
        // GET: api/Personas/SelectClientes
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> SelectClientes()
        {
            var persona = await _context.Persona.Where(x => x.tipo_persona == "Cliente").ToListAsync();

            return persona.Select(x => new SelectViewModel
            {
                idpersona = x.idpersona,
                nombre = x.nombre

            });

        }


        [Authorize(Roles = "Vendedor,Administrador,Almacenero")]
        // POST: api/Personas/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var email = model.email.ToLower();
            if (await _context.Persona.AnyAsync(x => x.email == email))
            {
                return BadRequest("El email ya existe");
            }
            Persona obj = new Persona
            {
                tipo_persona = model.tipo_persona,
                nombre = model.nombre,
                tipo_documento = model.tipo_documento,
                num_documento = model.num_documento,
                direccion = model.direccion,
                telefono = model.telefono,
                email = model.email.ToLower()
            };
            _context.Persona.Add(obj);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        [Authorize(Roles = "Vendedor,Administrador,Almacenero")]
        // PUT: api/Personas/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idpersona <= 0)
            {
                return BadRequest();
            }
            var persona = await _context.Persona.FirstOrDefaultAsync(x => x.idpersona == model.idpersona);
            if (persona == null)
            {
                return NotFound();
            }

            persona.tipo_persona = model.tipo_persona;
            persona.nombre = model.nombre;
            persona.tipo_documento = model.tipo_documento;
            persona.num_documento = model.num_documento;
            persona.direccion = model.direccion;
            persona.telefono = model.telefono;
            persona.email = model.email;

            _context.Entry(persona).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }
        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.idpersona == id);
        }
    }
}