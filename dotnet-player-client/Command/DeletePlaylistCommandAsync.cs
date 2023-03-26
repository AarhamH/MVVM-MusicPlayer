using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dotnet_player_client.Command
{
    public class DeletePlaylistCommandAsync: BaseCommandAsync
    {
        private readonly IPlayerService _musicService;
        private readonly PLStorage _plStorage;
        private readonly SongStorage _songStorage;
        private readonly INavigationService _navigationService;
        private readonly BrowserNavStorage _browserNavStorage;
        private readonly ObservableCollection<PLModel>? _observablePL;

        public DeletePlaylistCommandAsync(IPlayerService musicService, PLStorage plStorage, SongStorage songStorage, INavigationService navigationService, BrowserNavStorage browserNavStorage)
        {
            _musicService = musicService;
            _plStorage = plStorage;
            _songStorage = songStorage;
            _navigationService = navigationService;
            _browserNavStorage = browserNavStorage;
        }

        public DeletePlaylistCommandAsync(IPlayerService musicService, PLStorage plStorage, SongStorage songStorage, INavigationService navigationService, BrowserNavStorage browserNavStorage, ObservableCollection<PLModel> observablePL) : this(musicService, plStorage, songStorage, navigationService, browserNavStorage)
        {
            _observablePL = observablePL;
        }

    }
}
