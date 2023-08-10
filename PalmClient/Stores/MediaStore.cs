using Microsoft.EntityFrameworkCore;
using PalmClient.Events;
using PalmClient.Models;
using dotnet_player_data.Data;
using dotnet_player_data.DataEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Stores
{
    public class MediaStore
    {
        public event EventHandler<PlaylistSongsAddedEventArgs>? PlaylistSongsAdded;

        private readonly List<MediaEntity> _songs;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;

        public IEnumerable<MediaEntity> Songs => _songs;

        public MediaStore(IDbContextFactory<DataContext> dbContextFactory)
        {
            _songs = new List<MediaEntity>();
            _dbContextFactory = dbContextFactory;
            Load();
        }

        public void Load()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                _songs.AddRange(dbContext.Songs.ToList());
            }
        }

        public async Task<bool> Add(MediaEntity media)
        {
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    dbContext.Songs.Add(media);
                    await dbContext.SaveChangesAsync();

                    _songs.Add(media);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> AddRange(IEnumerable<MediaEntity> medias, bool raiseAddEvent = false)
        {
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    dbContext.Songs.AddRange(medias);
                    await dbContext.SaveChangesAsync();

                    _songs.AddRange(medias);

                    if (raiseAddEvent)
                        PlaylistSongsAdded?.Invoke(this, new PlaylistSongsAddedEventArgs(medias));
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> RemoveAll(Func<MediaEntity, bool> predicate)
        {
            _songs.RemoveAll(x => predicate.Invoke(x));
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    var itemsRemove = dbContext.Songs.Where(predicate);
                    dbContext.Songs.RemoveRange(itemsRemove);
                    Load();
                    await dbContext.SaveChangesAsync();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> Remove(int mediaId)
        {
            _songs.RemoveAll(x => x.Id == mediaId);
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    dbContext.Songs.Remove(new MediaEntity { Id = mediaId });
                    await dbContext.SaveChangesAsync();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
