using EventManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data
{
    /// <summary>
    /// EF Core database context.
    /// Inherits from IdentityDbContext to include all ASP.NET Identity tables
    /// (Users, Roles, Claims, etc.) alongside our custom Events table.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Represents the Events table in the SQLite database
        public DbSet<Event> Events { get; set; }
    }
}
