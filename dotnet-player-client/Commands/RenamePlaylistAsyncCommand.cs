using dotnet_player_client.Extensions;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_data.DataEntities;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Commands
{
    public class RenamePlaylistAsyncCommand : AsyncCommandBase
    {
        private readonly PlaylistStore _playlistStore;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserNavigationStore;

        public RenamePlaylistAsyncCommand(PlaylistStore playlistStore, PlaylistBrowserNavigationStore playlistBrowserNavigationStore)
        {
            _playlistStore = playlistStore;
            _playlistBrowserNavigationStore = playlistBrowserNavigationStore;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {

            if(parameter is string playlistName)
            {
                await _playlistStore.Rename(_playlistBrowserNavigationStore.BrowserPlaylistId, playlistName);
            }
        }
    }
}
