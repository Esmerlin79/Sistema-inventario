using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.Almacen
{
    public class ArticuloMap : IEntityTypeConfiguration<articulo>
    {
        public void Configure(EntityTypeBuilder<articulo> builder)
        {
            builder.ToTable("articulo")
                .HasKey(x => x.idarticulo);
        }
    }
}
