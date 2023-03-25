using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_data.Objects;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class CreatePlaylistCommandAsync : BaseCommandAsync
    {
        private readonly PLStorage _playlistStore;
        private readonly ObservableCollection<PLModel>? _observablePlaylists;
        public CreatePlaylistCommandAsync(PLStorage playlistStore)
        {
            _playlistStore = playlistStore;
        }

        public CreatePlaylistCommandAsync(PLStorage playlistStore, ObservableCollection<PLModel> observablePlaylists) : this(playlistStore)
        {
            _observablePlaylists = observablePlaylists;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            var playlistId = _playlistStore.PlayList.Count() + 1;

            var playlist = new PlayListObject
            {
                PLTitle = $"My Playlist #{playlistId}",
            };

            await _playlistStore.Append(playlist);

            _observablePlaylists?.Insert(0, new PLModel
            {
                ID = playlist.Id,
                Name = playlist.PLTitle,
            });
        }
    }
}
