using Microsoft.EntityFrameworkCore;

namespace SqlApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Add your DbSets here, for example:
        
          }
}