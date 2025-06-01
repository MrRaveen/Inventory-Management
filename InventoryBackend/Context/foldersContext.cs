using InventoryBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryBackend.Context
{
    public class foldersContext : DbContext
    {
        public foldersContext(DbContextOptions<foldersContext> options) : base(options)
        {

        }
        public DbSet<folders> folders => Set<folders>();
    }
}
