using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.Almacen
{
    public class CategoriaMap : IEntityTypeConfiguration<categoria>
    {
        public void Configure(EntityTypeBuilder<categoria> builder)
        {
            builder.ToTable("categoria")
                .HasKey(x => x.idcategoria);
            builder.Property(x => x.nombre)
                .HasMaxLength(50);
            builder.Property(x => x.descripcion)
                .HasMaxLength(256);
                
        }
    }
}
