using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.Almacen
{
    public class IngresoMap : IEntityTypeConfiguration<ingreso>
    {
        public void Configure(EntityTypeBuilder<ingreso> builder)
        {
            builder.ToTable("ingreso")
                .HasKey(x => x.idingreso);
            builder.HasOne(x => x.Persona)
                .WithMany(x => x.ingreso)
                .HasForeignKey(x => x.idproveedor);
        }
    }
}
