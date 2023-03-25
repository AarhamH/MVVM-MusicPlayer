using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnet_player_client.Command;
using dotnet_player_client.Services;
using dotnet_player_data.Context;
using dotnet_player_data.Objects;
using dotnet_player_client.Models;
using dotnet_player_client.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace dotnet_player_client.ViewModels
{
    public class DownloadVM : VMBase
    {
        private readonly IPlayerService _musicService;
        private readonly SongStorage _mediaStorage;
        public ObservableCollection<YoutubeModel> ResMedia;

        private bool _isLoadingSearch;
        private bool _isFailedSearch;
        private string? _searchText;

        public bool IsLoadingSearch
        {
            get => _isLoadingSearch;
            set
            {
                _isLoadingSearch = value;
                OnPropertyChanged();
            }
        }

        public bool IsFailedSearch
        {
            get => _isFailedSearch;
            set
            {
                _isFailedSearch = value;
                OnPropertyChanged();
            }
        }

        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchSong { get; set; }
        public ICommand DownloadSong { get; set; }

        public DownloadVM(SongStorage mediaStorage, IPlayerService musicService, IYouTubeClientService youTubeClientService, INavigationService navigationService, BrowserNavStorage browserNavStorage)
        {
            _musicService = musicService;
            ResMedia = new ObservableCollection<YoutubeModel>();
            SearchSong = new SearchYoutubeCommandAsync(youTubeClientService, ResMedia, this);
            _mediaStorage = mediaStorage;
            // create Download method to assign to DownloadSong:
            
        }
    }
}
