using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class GoToDownloadCommanAsync : BaseCommand
    {
        private readonly INavigationService _navigationService;
        private readonly BrowserNavStorage _broswerNavStorage;

        public GoToDownloadCommanAsync(INavigationService navigationService, BrowserNavStorage broswerNavStorage)
        {
            _navigationService = navigationService;
            _broswerNavStorage = broswerNavStorage;
        }

        public override void Execute(object? parameter)
        {
            _broswerNavStorage.BrowserPlaylistID = 0;
            _navigationService.NavigateDownloads();
        }
    }
}
