using Castle.Components.DictionaryAdapter.Xml;
using dotnet_player_client.Arguments;
using dotnet_player_client.Command;
using dotnet_player_client.Enumeration;
using dotnet_player_client.FileDropInterface;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_client.Utilities;
using dotnet_player_data.Objects;
using MusicPlayerClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace dotnet_player_client.ViewModels
{
    public class ToolbarVM : VMBase
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

            _musicService = musicService;
            musicService.MusicPlayerEvent += OnMusicPlayerEvent;

            _navigationService = navigationService;
            CurrentPage = navigationService.CurrentPage;
            navigationService.PageChangedEvent += OnPageChangedEvent;

            PlayLists = new ObservableCollection<PLModel>(plStorage.PlayList.Select(x => new PLModel
            {
                ID = x.Id,
                Name = x.PLTitle,
            }).Reverse().ToList());

            _browserNavStorage = browserNavStorage;
            browserNavStorage.PLBrowse += OnPlaylistBrowserChanged;

            ToggleRemoveActive = new TogglePlaylistRemoveCommand(this);
            NavigateHome = new GoToHomeCommand(navigationService, browserNavStorage);
            NavigatePlaylist = new GoToPlaylistCommand(navigationService, browserNavStorage);
            NavigateDownloads = new GoToDownloadCommand(navigationService, browserNavStorage);
            DeletePlaylist = new DeletePlaylistCommandAsync(musicService, plStorage, songStorage, navigationService, browserNavStorage, PlayLists);
            CreatePlaylist = new CreatePlaylistCommandAsync(plStorage, PlayLists);
        }
        

        private void OnPlaylistNameChanged(object? sender, PLNameArgs args) 
        { 
            var playlist = PlayLists.FirstOrDefault(x => x.ID == args.ID);
            if (playlist != null)
            {
                playlist.Name = args.Name;
            }
        }

        private void OnPlaylistBrowserChanged(object? sender, PLBrowseArgs args)
        {
            PlayLists.ToList().ForEach(x =>
            {
                if (x.ID == args.PlayListID)
                {
                    x.IsSelected = true;
                }
                else
                {
                    x.IsSelected = false;
                }
            });
        }

        private void OnMusicPlayerEvent(object? sender, SongArgs e)
        {
            switch (e.FuncType)
            {
                case PlayerFuncType.Playing:
                    PlayLists.ToList().ForEach(x =>
                    {
                        if (x.ID == e.SongObj?.ListID)
                        {
                            x.IsPlaying = true;
                        }
                        else
                        {
                            x.IsPlaying = false;
                        }
                    });
                    break;
                default:
                    PlayLists.ToList().ForEach(x => x.IsPlaying = false);
                    break;
            }
        }

        private void OnPageChangedEvent(object? sender, PageChangeArgs args)
        {
            CurrentPage = args.Page;
        }

        public async Task OnFilesDroppedAsync(string[] files, object? parameter)
        {
            if (parameter is int playlistId)
            {
                var mediaEntities = files.Where(x => PathUtil.HasAudioVideoExtensions(x)).Select(x => new SongObjects
                {
                    Path = x,
                    ListID = playlistId
                }).ToList();

                await _songStorage.AppendRange(mediaEntities, true);
            }
            else // Add to main playlist
            {
                var mediaEntities = files.Where(x => PathUtil.HasAudioVideoExtensions(x)).Select(x => new SongObjects
                {
                    Path = x
                }).ToList();

                await _songStorage.AppendRange(mediaEntities, true);
            }
        }

        public override void Dispose()
        {
            _plStorage.PLName -= OnPlaylistNameChanged;
            _browserNavStorage.PLBrowse -= OnPlaylistBrowserChanged;
            _musicService.MusicPlayerEvent -= OnMusicPlayerEvent;
            _navigationService.PageChangedEvent -= OnPageChangedEvent;
        }
    }
}
