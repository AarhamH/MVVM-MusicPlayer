using dotnet_player_client.Extensions;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_data.DataEntities;
using NAudio.Wave;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace dotnet_player_client.Commands
{
    public class CreatePlaylistAsyncCommand : AsyncCommandBase
    {
        private readonly PlaylistStore _playlistStore;
        private readonly ObservableCollection<PlaylistModel>? _observablePlaylists;
        public CreatePlaylistAsyncCommand(PlaylistStore playlistStore)
        {
            _playlistStore = playlistStore;
        }

        public CreatePlaylistAsyncCommand(PlaylistStore playlistStore, ObservableCollection<PlaylistModel> observablePlaylists) : this(playlistStore)
        {
            _observablePlaylists = observablePlaylists;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            var playlistId = _playlistStore.Playlists.Count() + 1;
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\banners" + "\\default.jpg";

            var playlist = new PlaylistEntity
            {
                Name = $"My Playlist #{playlistId}",
                CreationDate = DateTime.Now,
                Banner = path
            };

            await _playlistStore.Add(playlist);


            _observablePlaylists?.Insert(0, new PlaylistModel
            {
                Id = playlist.Id,
                Name = playlist.Name,
                CreationDate = playlist.CreationDate,
                Banner = playlist.Banner
            });
        }
    }
}
