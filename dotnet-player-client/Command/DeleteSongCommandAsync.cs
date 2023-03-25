using dotnet_player_client.Utilities;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class DeleteSongCommandAsync : BaseCommandAsync
    {
        private readonly IPlayerService _musicService;
        private readonly SongStorage _mediaStore;
        private readonly ObservableCollection<SongModel>? _observableSongs;
        public DeleteSongCommandAsync(IPlayerService musicService, SongStorage mediaStore)
        {
            _musicService = musicService;
            _mediaStore = mediaStore;
        }

        public DeleteSongCommandAsync(IPlayerService musicService, SongStorage mediaStore, ObservableCollection<SongModel> observableSongs) : this(musicService, mediaStore)
        {
            _observableSongs = observableSongs;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is int SongId)
            {
                if (_musicService.CurrentSong?.Id == SongId)
                {
                    _musicService.Stop();
                }

                _observableSongs?.RemoveAll(x => x.Id == SongId);

                await _mediaStore.DestroyOne(SongId);
            }
        }
    }
}
