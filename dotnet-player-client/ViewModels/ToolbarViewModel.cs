using Microsoft.EntityFrameworkCore;
using dotnet_player_client.Commands;
using dotnet_player_client.Core;
using dotnet_player_client.Enums;
using dotnet_player_client.Events;
using dotnet_player_client.Extensions;
using dotnet_player_client.Interfaces;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_data.Data;
using dotnet_player_data.DataEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace dotnet_player_client.ViewModels
{
    public class ToolbarViewModel : ViewModelBase, IFilesDropAsync
    {
        private readonly PlaylistStore _playlistStore;
        private readonly MediaStore _mediaStore;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserStore;
        private readonly IMusicPlayerService _musicPlayerService;
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
        public ObservableCollection<PlaylistModel> Playlists { get; set; }

        public ToolbarViewModel(IMusicPlayerService musicPlayerService, INavigationService navigationService, PlaylistBrowserNavigationStore playlistBrowserStore, PlaylistStore playlistStore, MediaStore mediaStore)
        {
            _playlistStore = playlistStore;
            _mediaStore = mediaStore;

            TogglePlayer = new ToggleMusicPlayerStateCommand(musicPlayerService);

            playlistStore.PlaylistNameChanged += OnPlaylistNameChanged;

            _musicPlayerService = musicPlayerService;
            musicPlayerService.MusicPlayerEvent += OnMusicPlayerEvent;

            _navigationService = navigationService;
            CurrentPage = navigationService.CurrentPage;
            navigationService.PageChangedEvent += OnPageChangedEvent;

            Playlists = new ObservableCollection<PlaylistModel>(playlistStore.Playlists.Select(x => new PlaylistModel
            {
                Id = x.Id,
                Name = x.Name,
                CreationDate = x.CreationDate
            }).Reverse().ToList());

            _playlistBrowserStore = playlistBrowserStore;
            playlistBrowserStore.PlaylistBrowserChanged += OnPlaylistBrowserChanged;

            ToggleRemoveActive = new TogglePlaylistRemoveCommand(this);
            NavigateHome = new SwitchPageToHomeCommand(navigationService, playlistBrowserStore);
            NavigatePlaylist = new SwitchPageToPlaylistCommand(navigationService, playlistBrowserStore);
            NavigateDownloads = new SwitchPageToDownloadsCommand(navigationService, playlistBrowserStore);
            DeletePlaylist = new DeleteSpecificPlaylistAsyncCommand(musicPlayerService, navigationService, playlistBrowserStore, playlistStore, mediaStore, Playlists);
            CreatePlaylist = new CreatePlaylistAsyncCommand(playlistStore, Playlists);
        }

        private void OnPlaylistNameChanged(object? sender, PlaylistNameChangedEventArgs args)
        {
            var playlist = Playlists.FirstOrDefault(x => x.Id == args.Id);
            if (playlist != null)
            {
                playlist.Name = args.Name;
            }
        }

        private void OnPlaylistBrowserChanged(object? sender, PlaylistBrowserChangedEventArgs args)
        {
            Playlists.ToList().ForEach(x =>
            {
                if (x.Id == args.PlaylistId)
                {
                    x.IsSelected = true;
                }
                else
                {
                    x.IsSelected = false;
                }
            });
        }

        private void OnMusicPlayerEvent(object? sender, MusicPlayerEventArgs e)
        {
            switch (e.Type)
            {
                case PlayerEventType.Playing:
                    Playlists.ToList().ForEach(x =>
                    {
                        if (x.Id == e.Media?.PlayerlistId)
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
                    Playlists.ToList().ForEach(x => x.IsPlaying = false);
                    break;
            }
        }

        private void OnPageChangedEvent(object? sender, PageChangedEventArgs args)
        {
            CurrentPage = args.Page;
        }

        public async Task OnFilesDroppedAsync(string[] files, object? parameter)
        {
            if (parameter is int playlistId)
            {
                var mediaEntities = files.Where(x => PathExtension.HasAudioVideoExtensions(x)).Select(x => new MediaEntity
                {
                    FilePath = x,
                    PlayerlistId = playlistId
                }).ToList();

                await _mediaStore.AddRange(mediaEntities, true);
            }
            else // Add to main playlist
            {
                var mediaEntities = files.Where(x => PathExtension.HasAudioVideoExtensions(x)).Select(x => new MediaEntity
                {
                    FilePath = x
                }).ToList();

                await _mediaStore.AddRange(mediaEntities, true);
            }
        }

        public override void Dispose()
        {
            _playlistStore.PlaylistNameChanged -= OnPlaylistNameChanged;
            _playlistBrowserStore.PlaylistBrowserChanged -= OnPlaylistBrowserChanged;
            _musicPlayerService.MusicPlayerEvent -= OnMusicPlayerEvent;
            _navigationService.PageChangedEvent -= OnPageChangedEvent;
        }
    }
}
