using DesafioWiPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioWiPro.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
           : base(options)
        {
        }
        public DbSet<Carteira> Carteiras { get; set; }
    }
}
