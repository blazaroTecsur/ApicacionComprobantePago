using ComprobantePago.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComprobantePago.Tests.Helpers
{
    public static class DbContextFactory
    {
        public static AppDbContext Crear(string? nombre = null)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(nombre ?? Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }
    }
}
