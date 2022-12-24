using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Models;

namespace Persistencia.FluentConfig.Administrador
{
    public class ParametroConfig
    {
        public ParametroConfig(EntityTypeBuilder<Parametro> entity)
        {
            entity.ToTable("Parametro");
            entity.HasKey(p => p.Id);

            entity.HasMany(p => p.ParametroDetalles)
                   .WithOne(p => p.Parametro);

            entity.Property(p=> p.VcNombre).IsRequired().HasMaxLength(100);

            entity.Property(p=> p.VcCodigoInterno).IsRequired().HasMaxLength(50);

            entity.Property(p => p.BEstado).IsRequired();

            entity.Property(p=> p.DtFechaCreacion).IsRequired();

            entity.Property(p=> p.DtFechaActualizacion).IsRequired(false);

            entity.Property(p=> p.DtFechaAnulacion).IsRequired(false);

        }
    }
}