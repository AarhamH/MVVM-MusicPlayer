using Microsoft.EntityFrameworkCore;
using dotnet_player_data.Objects;

namespace dotnet_player_data.Context
{
    public class DataContext: DbContext
    {
        public DbSet<PlayListObject> PlayListObjects { get; set; }
        public DbSet<SongObjects> SongObjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlite(@"Data source=data/player");
        }
    }
}
