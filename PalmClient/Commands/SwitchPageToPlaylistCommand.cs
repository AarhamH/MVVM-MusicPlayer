using PalmClient.Services;
using PalmClient.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Commands
{
    public class SwitchPageToPlaylistCommand : CommandBase
    {
        private readonly INavigationService _navigationService;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserNavigationStore;
        public SwitchPageToPlaylistCommand(INavigationService navigationService, PlaylistBrowserNavigationStore playlistBrowserNavigationStore)
        {
            _navigationService = navigationService;
            _playlistBrowserNavigationStore = playlistBrowserNavigationStore;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is int playlistId)
            {
                if (_playlistBrowserNavigationStore.BrowserPlaylistId != playlistId)
                {
                    _playlistBrowserNavigationStore.BrowserPlaylistId = playlistId;
                    _navigationService.NavigatePlaylist();
                }
            }
        }
    }
}
