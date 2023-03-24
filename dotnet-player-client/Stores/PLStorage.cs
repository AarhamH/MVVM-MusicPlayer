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
    public class PLStorage
    {
        public event EventHandler<PLNameArgs>? PLName;

        private readonly List<PlayListObject> _playList;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;


        public IEnumerable<PlayListObject> PlayList => _playList;

        public void Fill()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                _playList.AddRange(dbContext.PlayListObjects.ToList());
            }
        }

        public async Task Rename(int playListID, string newTitle)
        {
            using (var dbContext =  await _dbContextFactory.CreateDbContextAsync())
            {
                var dbPlayList = await dbContext.PlayListObjects.FindAsync(playListID);
                if(dbPlayList != null)
                {
                    dbPlayList.PLTitle = newTitle;
                    await dbContext.SaveChangesAsync();

                    var playList = _playList.FirstOrDefault(x => x.Id == playListID);
                    if(playList != null)
                    {
                        playList.PLTitle = newTitle;
                    }

                    PLName?.Invoke(this, new PLNameArgs(playListID, newTitle));
                }
            }
            
        }

        public async Task<bool> Append(PlayListObject pl)
        {
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    dbContext.PlayListObjects.Add(pl);
                    await dbContext.SaveChangesAsync();
                }
                catch { return false; }
            }
            return true;
        }

        public async Task<bool> DestroyOne(int playListID)
        {
            _playList.RemoveAll(x => x.Id == playListID);
            using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
            {
                try
                {
                    dbContext.SongObjects.Remove(new SongObjects { Id = playListID  });
                    await dbContext.SaveChangesAsync();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public PLStorage(IDbContextFactory<DataContext> dbContextFactory)
        {
            _playList = new List<PlayListObject>();
            _dbContextFactory = dbContextFactory;
            Fill();
        }
    }
}
