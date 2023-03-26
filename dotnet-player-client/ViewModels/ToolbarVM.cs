using dotnet_player_client.Arguments;
using dotnet_player_client.Command;
using dotnet_player_client.Enumeration;
using dotnet_player_client.FileDropInterface;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dotnet_player_client.ViewModels
{
    public class ToolbarVM : VMBase, IFileDrop
    {
        private readonly PLStorage _plStorage;
        private readonly SongStorage _songStorage;
        private readonly BrowserNavStorage _browserNavStorage;
        private readonly IPlayerService _musicService;
        private readonly INavigationService _navigationService;

        private PageType _currentPage;

        public PageType CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        private bool _isRemoveActive;
        public bool IsRemoveActive
        {
            get => _isRemoveActive;
            set
            {
                _isRemoveActive = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleRemoveActive { get; }
        public ICommand DeletePlaylist { get; }
        public ICommand NavigatePlaylist { get; }
        public ICommand NavigateDownloads { get; }
        public ICommand NavigateHome { get; }
        public ICommand CreatePlaylist { get; }
        public ICommand TogglePlayer { get; }

        public ObservableCollection<PLModel> PlayLists { get; set; }

        public ToolbarVM(PLStorage plStorage, SongStorage songStorage, BrowserNavStorage browserNavStorage, IPlayerService musicService, INavigationService navigationService)
        {
            _plStorage = plStorage;
            _songStorage = songStorage;

            TogglePlayer = new TogglePlayerCommand(musicService);
            plStorage.PLName += OnPlaylistNameChanged;
        }
        

        private void OnPlaylistNameChanged(object? sender, PLNameArgs args) 
        { 
            var playlist = PlayLists.FirstOrDefault(x => x.ID == args.ID);
            if (playlist != null)
            {
                playlist.Name = args.Name;
            }
        }
    }
}
