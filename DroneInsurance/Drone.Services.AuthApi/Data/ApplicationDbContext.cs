using Drone.Services.AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Drone.Services.AuthApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
        {

        }
        public DbSet<LoginUser> Users { get; set; }

    
}
}
