using PalmClient.Services;
using PalmClient.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Commands
{
    public class SwitchPageToHomeCommand : CommandBase
    {
        private readonly INavigationService _navigationService;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserNav;
        public SwitchPageToHomeCommand(INavigationService navigationService, PlaylistBrowserNavigationStore playlistBrowserNav)
        {
            _navigationService = navigationService;
            _playlistBrowserNav = playlistBrowserNav;
        }

        public override void Execute(object? parameter)
        {
            _playlistBrowserNav.BrowserPlaylistId = 0;
            _navigationService.NavigateHome();
        }
    }
}
