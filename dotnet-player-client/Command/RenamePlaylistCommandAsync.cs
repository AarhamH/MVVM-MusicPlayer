using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnet_player_client.Utilities;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;


namespace dotnet_player_client.Command
{
    public class RenamePlaylistCommandAsync : BaseCommandAsync
    {
        private readonly PLStorage _playlistStorage;
        private readonly BrowserNavStorage _browserNavStorage;

        public RenamePlaylistCommandAsync(PLStorage pLStorage, BrowserNavStorage browserNavStorage)
        {
            _playlistStorage = pLStorage;
            _browserNavStorage = browserNavStorage;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            if(parameter is string playListName)
            {
                await _playlistStorage.Rename(_browserNavStorage.BrowserPlaylistID, playListName);
            }
        }
    }
}
