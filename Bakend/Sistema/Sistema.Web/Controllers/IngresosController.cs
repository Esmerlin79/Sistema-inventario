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
using Sistema.Web.Models.Almacen.Ingreso;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public IngresosController(DbContextSistema context)
        {
            _context = context;
        }


        // GET: api/Ingresos/Listar
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<IngresosViewModel>> Listar()
        {
            var ingreso = await _context.ingreso.Include(x => x.Usuario).Include(x => x.Persona)
                 .OrderByDescending(x => x.idingreso).Take(100).ToListAsync();

            return ingreso.Select(x => new IngresosViewModel
            {
                idingreso = x.idingreso,
                idproveedor = x.idproveedor,
                proveedor = x.Persona.nombre,
                idusuario = x.idusuario,
                usuario = x.Usuario.nombre,
                tipo_comprobante = x.tipo_comprobante,
                serie_comprobante = x.serie_comprobante,
                num_comprobante = x.num_comprobante,
                fecha_hora = x.fecha_hora,
                impuesto = x.impuesto,
                total = x.total,
                estado = x.estado
            });

        }

        // GET: api/Ingresos/ListarFiltro/texto
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<IngresosViewModel>> ListarFiltro([FromRoute] string texto)
        {
            var ingreso = await _context.ingreso.Include(x => x.Usuario).Include(x => x.Persona)
                 .Where(x => x.num_comprobante.Contains(texto)).OrderByDescending(x => x.idingreso)
                 .ToListAsync();

            return ingreso.Select(x => new IngresosViewModel
            {
                idingreso = x.idingreso,
                idproveedor = x.idproveedor,
                proveedor = x.Persona.nombre,
                idusuario = x.idusuario,
                usuario = x.Usuario.nombre,
                tipo_comprobante = x.tipo_comprobante,
                serie_comprobante = x.serie_comprobante,
                num_comprobante = x.num_comprobante,
                fecha_hora = x.fecha_hora,
                impuesto = x.impuesto,
                total = x.total,
                estado = x.estado
            });

        }

        // GET: api/Ingresos/ListarDetalles/2
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{idingreso}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idingreso)
        {
            var detalle = await _context.DetalleIngreso.Include(x => x.articulo)
                .Where(x => x.idingreso == idingreso)
                .ToListAsync();

            return detalle.Select(x => new DetalleViewModel
            {
                idarticulo = x.idarticulo,
                articulo = x.articulo.nombre,
                cantidad = x.cantidad,
                precio = x.precio,
            });

        }
       
        // POST: api/Ingresos/Crear
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fechaHora = DateTime.Now;
            ingreso ingreso = new ingreso()
            {
                idproveedor = model.idproveedor,
                idusuario = model.idusuario,
                tipo_comprobante = model.tipo_comprobante,
                serie_comprobante = model.serie_comprobante,
                num_comprobante = model.num_comprobante,
                fecha_hora = fechaHora,
                impuesto = model.impuesto,
                total = model.total,
                estado = "Aceptado",
            };
            
            try
            {
                _context.ingreso.Add(ingreso);
                await _context.SaveChangesAsync();

                var id = ingreso.idingreso;

                foreach (var detalleView in model.detalles)
                {
                    DetalleIngreso detalles = new DetalleIngreso()
                    {
                        idingreso = id,
                        idarticulo = detalleView.idarticulo,
                        cantidad = detalleView.cantidad,
                        precio = detalleView.precio

                    };
                    _context.DetalleIngreso.Add(detalles);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // PUT: api/Ingresos/Anular/1
        [Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var ingreso = await _context.ingreso.FirstOrDefaultAsync(x => x.idingreso == id);
            if (ingreso == null)
            {
                return NotFound();
            }
            ingreso.estado = "Anulado";

            _context.Entry(ingreso).State = EntityState.Modified; 
            try
            {
                await _context.SaveChangesAsync();

                var detalle = await _context.DetalleIngreso.Include(x => x.articulo)
                    .Where(x => x.idingreso == id).ToListAsync();
                foreach(var item in detalle)
                {
                    var articulo = await _context.articulo.FirstOrDefaultAsync(x => x.idarticulo == item.idarticulo);
                    articulo.stock = item.articulo.stock - item.cantidad;
                   
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }
        private bool ingresoExists(int id)
        {
            return _context.ingreso.Any(e => e.idingreso == id);
        }
    }
}