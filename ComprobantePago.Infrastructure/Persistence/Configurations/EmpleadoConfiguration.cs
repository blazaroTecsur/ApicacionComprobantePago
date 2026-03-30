using ComprobantePago.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComprobantePago.Infrastructure.Persistence.Configurations
{
    public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.ToTable("rcoempleado");
            builder.HasKey(x => x.IdEmpleado);
            builder.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
        }
    }
}
