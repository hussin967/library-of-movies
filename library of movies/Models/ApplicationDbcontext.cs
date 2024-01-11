using Microsoft.EntityFrameworkCore;

namespace library_of_movies.Models
{
    public class ApplicationDbcontext : DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        { }
            public DbSet<Genre> Genres { get; set; }

        public DbSet<Movie> Movies { get; set; }

    }
}
