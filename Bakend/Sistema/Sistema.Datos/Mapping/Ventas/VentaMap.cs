using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Ventas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.Ventas
{
    public class VentaMap : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("venta")
                .HasKey(x => x.idventa);
            builder.HasOne(x => x.persona)
                .WithMany(x => x.ventas)
                .HasForeignKey(x => x.idcliente);
        }
    }
}
