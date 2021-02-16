using Microsoft.EntityFrameworkCore;
using Sistema.Datos.Mapping.Almacen;
using Sistema.Datos.Mapping.Usuarios;
using Sistema.Datos.Mapping.Ventas;
using Sistema.Entidades.Almacen;
using Sistema.Entidades.Usuarios;
using Sistema.Entidades.Ventas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos
{
    public class DbContextSistema : DbContext
    {
        public DbSet<categoria> categoria { get; set; }
        public DbSet<articulo> articulo { get; set; }
        public DbSet<rol> rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<ingreso> ingreso { get; set; }
        public DbSet<DetalleIngreso> DetalleIngreso { get; set; }
        public DbSet<Venta> ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {
                
        }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ArticuloMap());
            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PersonaMap());
            modelBuilder.ApplyConfiguration(new IngresoMap());
            modelBuilder.ApplyConfiguration(new DetalleIngresoMap());
            modelBuilder.ApplyConfiguration(new VentaMap());
            modelBuilder.ApplyConfiguration(new DetalleVentaMap());

        }
    }
}
