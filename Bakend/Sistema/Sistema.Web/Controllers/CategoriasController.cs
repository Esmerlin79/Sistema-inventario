using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Datos.Repository.Interface;
using Sistema.Datos.Services;
using Sistema.Entidades.Almacen;
using Sistema.Web.Models.Almacen.Categoria;

namespace Sistema.Web.Controllers
{
    //[Authorize(Roles = "Almacenero,Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        private readonly ICategoria _service;
        public CategoriasController(DbContextSistema context, ICategoria service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/Categorias/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<CategoriaViewModel>> Listar()
        {
          //  var cat =  await _context.categoria.ToListAsync();

            var listadoService = await _service.getCategoria();

            List<categoria> listado = new List<categoria>();
            foreach(var item in listadoService)
            {
                listado.Add(item);
            }

            return listado.Select(x => new CategoriaViewModel
            {
                idcategoria = x.idcategoria,
                nombre = x.nombre,
                descripcion = x.descripcion,
                condicion = x.condicion
            });
           
        }

        // GET: api/Categorias/Select
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> Select()
        {
            var cat = await _context.categoria.Where(x => x.condicion == true).ToListAsync();

            return cat.Select(x => new SelectViewModel
            {
                idcategoria = x.idcategoria,
                nombre = x.nombre,

            });

        }

        // GET: api/Categorias/Mostrar/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {
            var categoria = await _context.categoria.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(new CategoriaViewModel { 
            idcategoria = categoria.idcategoria,
            nombre = categoria.nombre,
            descripcion = categoria.descripcion,
            condicion = categoria.condicion
            });
        }

        // PUT: api/Categorias/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idcategoria <= 0 )
            {
                return BadRequest();
            }
            var findCat = await _context.categoria.FirstOrDefaultAsync(x => x.idcategoria == model.idcategoria);
            if (findCat == null)
            {
                return NotFound();
            }
            findCat.nombre = model.nombre;
            findCat.descripcion = model.descripcion;
            _context.Entry(findCat).State = EntityState.Modified;
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

        // POST: api/Categorias/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            categoria obj = new categoria
            {
                nombre = model.nombre,
                descripcion = model.descripcion,
                condicion = true
            };
            _context.categoria.Add(obj);
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

        // DELETE: api/Categorias/Eliminar/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = await _context.categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.categoria.Remove(categoria);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(categoria);
        }

        // PUT: api/Categorias/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var findCat = await _context.categoria.FirstOrDefaultAsync(x => x.idcategoria == id);
            if (findCat == null)
            {
                return NotFound();
            }
            findCat.condicion = false;

            _context.Entry(findCat).State = EntityState.Modified;
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

        // PUT: api/Categorias/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var findCat = await _context.categoria.FirstOrDefaultAsync(x => x.idcategoria == id);
            if (findCat == null)
            {
                return NotFound();
            }
            findCat.condicion = true;

            _context.Entry(findCat).State = EntityState.Modified;
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
        private bool categoriaExists(int id)
        {
            return _context.categoria.Any(e => e.idcategoria == id);
        }
    }
}