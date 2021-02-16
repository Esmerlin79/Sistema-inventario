using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Almacen;
using Sistema.Web.Models.Almacen.Articulo;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ArticulosController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Articulos/Listar
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ArticuloViewModel>> Listar()
        {
            var articulo = await _context.articulo.Include(x => x.categoria).ToListAsync();

            return articulo.Select(x => new ArticuloViewModel
            {
                idarticulo = x.idarticulo,
                idcategoria = x.idcategoria,
                categoria = x.categoria.nombre,
                codigo = x.codigo,
                nombre = x.nombre,
                precio_venta = x.precio_venta,
                stock = x.stock,
                descripcion = x.descripcion,
                condicion = x.condicion
            });

        }

        // GET: api/Articulos/ListarIngreso/texto/sdsd
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarIngreso([FromRoute] string texto)
        {
            var articulo = await _context.articulo.Include(x => x.categoria)
                .Where(x => x.nombre.Contains(texto)).Where(x => x.condicion == true)
                .ToListAsync();

            return articulo.Select(x => new ArticuloViewModel
            {
                idarticulo = x.idarticulo,
                idcategoria = x.idcategoria,
                categoria = x.categoria.nombre,
                codigo = x.codigo,
                nombre = x.nombre,
                precio_venta = x.precio_venta,
                stock = x.stock,
                descripcion = x.descripcion,
                condicion = x.condicion
            });

        }

        // GET: api/Articulos/ListarIngreso/texto/sdsd
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarVenta([FromRoute] string texto)
        {
            var articulo = await _context.articulo.Include(x => x.categoria)
                .Where(x => x.nombre.Contains(texto)).Where(x => x.condicion == true)
                .Where(x => x.stock > 0).ToListAsync();

            return articulo.Select(x => new ArticuloViewModel
            {
                idarticulo = x.idarticulo,
                idcategoria = x.idcategoria,
                categoria = x.categoria.nombre,
                codigo = x.codigo,
                nombre = x.nombre,
                precio_venta = x.precio_venta,
                stock = x.stock,
                descripcion = x.descripcion,
                condicion = x.condicion
            });

        }

        // GET: api/Articulos/Mostrar/5
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {
            var articulo = await _context.articulo.Include(x=> x.categoria).SingleOrDefaultAsync(x=> x.idarticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre, 
                precio_venta = articulo.precio_venta,
                stock = articulo.stock,
                descripcion = articulo.descripcion,
                condicion = articulo.condicion
            });
        }

        // GET: api/Articulos/BuscarCodigoIngreso/00321232
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{codigo}")]
        public async Task<IActionResult> BuscarCodigoIngreso([FromRoute] string codigo)
        {
            var articulo = await _context.articulo.Include(x => x.categoria).Where(x => x.condicion == true)
                .SingleOrDefaultAsync(x => x.codigo == codigo);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                precio_venta = articulo.precio_venta,
                stock = articulo.stock,
                descripcion = articulo.descripcion,
                condicion = articulo.condicion
            });
        }

        // GET: api/Articulos/BuscarCodigoIngreso/00321232
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{codigo}")]
        public async Task<IActionResult> BuscarCodigoVenta([FromRoute] string codigo)
        {
            var articulo = await _context.articulo.Include(x => x.categoria).Where(x => x.condicion == true)
                .Where(x => x.stock > 0)
                .SingleOrDefaultAsync(x => x.codigo == codigo);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                precio_venta = articulo.precio_venta,
                stock = articulo.stock,
                descripcion = articulo.descripcion,
                condicion = articulo.condicion
            });
        }

        [Authorize(Roles = "Almacenero,Administrador")]
        // PUT: api/Articulos/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idarticulo <= 0)
            {
                return BadRequest();
            }
            var articulo = await _context.articulo.FirstOrDefaultAsync(x => x.idarticulo == model.idarticulo);
            if (articulo == null)
            {
                return NotFound();
            }
            articulo.idcategoria = model.idcategoria;
            articulo.codigo = model.codigo;
            articulo.nombre = model.nombre;
            articulo.precio_venta = model.precio_venta;
            articulo.stock = model.stock;
            articulo.descripcion = model.descripcion;
            _context.Update(articulo);
           // _context.Entry(articulo).State = EntityState.Modified;
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

        [Authorize(Roles = "Almacenero,Administrador")]
        // POST: api/Articulos/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            articulo obj = new articulo
            {
                idcategoria = model.idcategoria,
                codigo = model.codigo,
                nombre = model.nombre,
                precio_venta = model.precio_venta,
                stock = model.stock,
                descripcion = model.descripcion,
                condicion = true
            };
            _context.articulo.Add(obj);
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

        [Authorize(Roles = "Almacenero,Administrador")]
        // PUT: api/Articulos/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var articulos = await _context.articulo.FirstOrDefaultAsync(x => x.idarticulo == id);
            if (articulos == null)
            {
                return NotFound();
            }
            articulos.condicion = false;

            _context.Entry(articulos).State = EntityState.Modified;
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

        [Authorize(Roles = "Almacenero,Administrador")]
        // PUT: api/Articulos/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var articulos = await _context.articulo.FirstOrDefaultAsync(x => x.idarticulo == id);
            if (articulos == null)
            {
                return NotFound();
            }
            articulos.condicion = true;

            _context.Entry(articulos).State = EntityState.Modified;
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
        private bool articuloExists(int id)
        {
            return _context.articulo.Any(e => e.idarticulo == id);
        }
    }
}