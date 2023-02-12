using Archon.DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Archon.WebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<LoginModel> LoginProperties { get; set; }
    }
}
