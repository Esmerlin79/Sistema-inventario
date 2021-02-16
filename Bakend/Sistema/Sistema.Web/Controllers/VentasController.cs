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
using Sistema.Web.Models.Ventas.Venta;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public VentasController(DbContextSistema context)
        {
            _context = context;
        }
        // GET: api/Ventas/Listar
        [Authorize(Roles = "Vendedor,Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> Listar()
        {
            var ventas = await _context.ventas.Include(x => x.usuario).Include(x => x.persona)
                 .OrderByDescending(x => x.idventa).Take(100).ToListAsync();

            return ventas.Select(x => new VentaViewModel
            {
                idventa = x.idventa,
                idcliente = x.idcliente,
                cliente = x.persona.nombre,
                numDocumento = x.persona.num_documento,
                direccion = x.persona.direccion,
                telefono = x.persona.telefono,
                email = x.persona.email,
                idusuario = x.idusuario,
                usuario = x.usuario.nombre,
                tipo_comprobante = x.tipo_comprobante,
                serie_comprobante = x.serie_comprobante,
                num_comprobante = x.num_comprobante,
                fecha_hora = x.fecha_hora,
                impuesto = x.impuesto,
                total = x.total,
                estado = x.estado
            });

        }

        // GET: api/Ventas/VentasMes12
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ConsultaViewModel>> VentasMes12()
        {
            var consulta = await _context.ventas.Include(x => x.usuario)
                .GroupBy(x => x.fecha_hora.Month)
                .Select(x => new { etiqueta = x.Key, valor = x.Sum(v=>v.total)})
                .OrderByDescending(x => x.etiqueta).Take(12).ToListAsync();

            return consulta.Select(x => new ConsultaViewModel
            {
                etiqueta = x.etiqueta.ToString(),
                valor = x.valor

            });

        }

        // GET: api/Ventas/ListarFiltro/texto
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<VentaViewModel>> ListarFiltro([FromRoute] string texto)
        {
            var venta = await _context.ventas.Include(x => x.usuario).Include(x => x.persona)
                 .Where(x => x.num_comprobante.Contains(texto)).OrderByDescending(x => x.idventa)
                 .ToListAsync();

            return venta.Select(x => new VentaViewModel
            {
                idventa = x.idventa,
                idcliente = x.idcliente,
                cliente = x.persona.nombre,
                numDocumento = x.persona.num_documento,
                direccion = x.persona.direccion,
                telefono = x.persona.telefono,
                email = x.persona.email,
                idusuario = x.idusuario,
                usuario = x.usuario.nombre,
                tipo_comprobante = x.tipo_comprobante,
                serie_comprobante = x.serie_comprobante,
                num_comprobante = x.num_comprobante,
                fecha_hora = x.fecha_hora,
                impuesto = x.impuesto,
                total = x.total,
                estado = x.estado
            });

        }

        // GET: api/Ventas/ConsultaFechas/2020-20-2/2021-20-2
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{FechaInicio}/{FechaFin}")]
        public async Task<IEnumerable<VentaViewModel>> ConsultaFechas([FromRoute] DateTime FechaInicio, DateTime FechaFin)
        {
            var ventas = await _context.ventas.Include(x => x.usuario).Include(x => x.persona)
                .Where(x => x.fecha_hora >= FechaInicio)
                .Where(x => x.fecha_hora <= FechaFin)
                .OrderByDescending(x => x.idventa).Take(100).ToListAsync();

            return ventas.Select(x => new VentaViewModel
            {
                idventa = x.idventa,
                idcliente = x.idcliente,
                cliente = x.persona.nombre,
                numDocumento = x.persona.num_documento,
                direccion = x.persona.direccion,
                telefono = x.persona.telefono,
                email = x.persona.email,
                idusuario = x.idusuario,
                usuario = x.usuario.nombre,
                tipo_comprobante = x.tipo_comprobante,
                serie_comprobante = x.serie_comprobante,
                num_comprobante = x.num_comprobante,
                fecha_hora = x.fecha_hora,
                impuesto = x.impuesto,
                total = x.total,
                estado = x.estado
            });

        }
        // GET: api/Ventas/ListarDetalles/2
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{idventa}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idventa)
        {
            var detalle = await _context.DetalleVentas.Include(x => x.articulos)
                .Where(x => x.idventa == idventa)
                .ToListAsync();

            return detalle.Select(x => new DetalleViewModel
            {
                idarticulo = x.idarticulo,
                articulo = x.articulos.nombre,
                cantidad = x.cantidad,
                precio = x.precio,
                descuento = x.descuento
            });

        }

        // POST: api/Ventas/Crear
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fechaHora = DateTime.Now;
            Venta venta = new Venta()
            {
                idcliente = model.idcliente,
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
                _context.ventas.Add(venta);
                await _context.SaveChangesAsync();

                var id = venta.idventa;

                foreach (var detalleView in model.detalles)
                {
                    DetalleVenta detalles = new DetalleVenta()
                    {
                        idventa = id,
                        idarticulo = detalleView.idarticulo,
                        cantidad = detalleView.cantidad,
                        precio = detalleView.precio,
                        descuento = detalleView.descuento

                    };
                    _context.DetalleVentas.Add(detalles);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // PUT: api/Ventas/Anular/1
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var venta = await _context.ventas.FirstOrDefaultAsync(x => x.idventa == id);
            if (venta == null)
            {
                return NotFound();
            }
            venta.estado = "Anulado";

            _context.Entry(venta).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();

                var detalle = await _context.DetalleVentas.Include(x => x.articulos)
                    .Where(x => x.idventa == id).ToListAsync();
                foreach (var item in detalle)
                {
                    var articulo = await _context.articulo.FirstOrDefaultAsync(x => x.idarticulo == item.idarticulo);
                    articulo.stock = item.articulos.stock + item.cantidad;

                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }
       
        private bool VentaExists(int id)
        {
            return _context.ventas.Any(e => e.idventa == id);
        }
    }
}