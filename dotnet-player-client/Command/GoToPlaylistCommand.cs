using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class GoToPlaylistCommand : BaseCommand
    {
        private readonly INavigationService _navigationService;
        private readonly BrowserNavStorage _browserNavStorage;

        public GoToPlaylistCommand(INavigationService navigationService, BrowserNavStorage browserNavStorage)
        {
            _navigationService = navigationService;
            _browserNavStorage = browserNavStorage;
        }

        public override void Execute(object? parameter)
        {
            if(parameter is int playlistId)
            {
                if(_browserNavStorage.BrowserPlaylistID != playlistId)
                {
                    _browserNavStorage.BrowserPlaylistID = playlistId;
                    _navigationService.NavigatePlaylist();
                }
            }
        }
    }
}
