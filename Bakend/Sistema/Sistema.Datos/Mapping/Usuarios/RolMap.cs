using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.Usuarios
{
    public class RolMap : IEntityTypeConfiguration<rol>
    {
        public void Configure(EntityTypeBuilder<rol> builder)
        {
            builder.ToTable("rol")
                .HasKey(x => x.idrol);
        }
    }
}
