using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Models;

namespace Persistencia.FluentConfig.Administrador
{
    public class ParametroDetalleConfig
    {
        public ParametroDetalleConfig(EntityTypeBuilder<ParametroDetalle> entity)
        {
            entity.ToTable("ParametroDetalle");
            entity.HasKey(p => p.Id);
                   

            entity.HasOne(p => p.Parametro)
                  .WithMany(p => p.ParametroDetalles)

                  .HasForeignKey(p => p.ParametroId)
                  .HasConstraintName("FK_Parametro")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(p=> p.VcNombre).IsRequired().HasMaxLength(150);

            entity.Property(p=> p.TxDescripcion).IsRequired(false);

            entity.Property(p => p.IdPadre).IsRequired(false);

            entity.Property(p=> p.VcCodigoInterno).IsRequired(false).HasMaxLength(50);

            entity.Property(p=> p.DCodigoIterno).IsRequired(false).HasPrecision(17,3);

            entity.Property(p=> p.BEstado).IsRequired();

            entity.Property(p=> p.RangoDesde).IsRequired(false);

            entity.Property(p=> p.RangoHasta).IsRequired(false);
        }
    }
}