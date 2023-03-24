using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using dotnet_player_client.Arguments;
using dotnet_player_data.Context;
using dotnet_player_data.Objects;
using Microsoft.EntityFrameworkCore.Internal;


namespace dotnet_player_client.Stores
{
    public class SongStorage
    {
        public event EventHandler<PLAppendArgs>? PLAppended;

        private readonly List<SongObjects> _songs;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;

        public IEnumerable<SongObjects> Songs => _songs;

        public void Fill()
        {
            using(var dbContext = _dbContextFactory.CreateDbContext())
            {
                _songs.AddRange(dbContext.SongObjects.ToList());
            }
        }

        public async Task<bool> Append(SongObjects song)
        {
            using(var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    dbContext.SongObjects.Add(song);
                    await dbContext.SaveChangesAsync();
                }
                catch { return false; }
            }
            return true;
        }

        public async Task<bool> AppendRange(IEnumerable<SongObjects> songs, bool delay = false)
        {
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    dbContext.SongObjects.AddRange(songs);
                    await dbContext.SaveChangesAsync();

                    _songs.AddRange(songs);

                    if (delay)
                        PLAppended?.Invoke(this, new PLAppendArgs(songs));
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> DestroyAll(Func<SongObjects, bool> condition)
        {
            _songs.RemoveAll(x => condition.Invoke(x));
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var itemsRemove = dbContext.SongObjects.Where(condition);
                    dbContext.SongObjects.RemoveRange(itemsRemove);
                    await dbContext.SaveChangesAsync();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> DestroyOne(int songID)
        {
            _songs.RemoveAll(x => x.Id == songID);
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    dbContext.SongObjects.Remove(new SongObjects { Id = songID } );
                    await dbContext.SaveChangesAsync();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public SongStorage(IDbContextFactory<DataContext> dbContextFactory)
        {
            _songs = new List<SongObjects>();
            _dbContextFactory = dbContextFactory;
            Fill();
        }
    }


}
