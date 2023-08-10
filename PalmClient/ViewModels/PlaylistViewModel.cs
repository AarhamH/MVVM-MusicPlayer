using PalmClient.Commands;
using PalmClient.Core;
using PalmClient.Enums;
using PalmClient.Events;
using PalmClient.Extensions;
using PalmClient.Interfaces;
using PalmClient.Models;
using PalmClient.Services;
using PalmClient.Stores;
using dotnet_player_data.DataEntities;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PalmClient.ViewModels
{
    public class PlaylistViewModel : ViewModelBase, IFilesDropAsync
    {
        private readonly IMusicPlayerService _musicService;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserNavigationStore;
        private readonly MediaStore _mediaStore;
        private readonly PlaylistStore _playlistStore;
        public string CurrentDateString { get; }

        public string? _currentPlaylistName;
        public string? _bannerUrl;
        public string? CurrentPlaylistName
        {
            get => _currentPlaylistName;
            set
            {
                _currentPlaylistName = value;
                OnPropertyChanged();
            }
        }

        public string? BannerUrl 
        { 
            get { return _bannerUrl; }
            set
            {
                if(_bannerUrl != value)
                {
                    _bannerUrl = value;
                    OnPropertyChanged(nameof(BannerUrl));
                }
            }
        }

        public string PlaylistCreationDate { get; }

        public ObservableCollection<MediaModel>? AllSongsOfPlaylist { get; set; }
        public ICommand? RenamePlaylist { get; }
        public ICommand? PlaySong { get; }
        public ICommand? OpenExplorer { get; }
        public ICommand? AddSong { get; set; }
        public ICommand? ChangeBanner { get; set; }
        public ICommand? DeleteSong { get; set; }

        public PlaylistViewModel(IMusicPlayerService musicService, INavigationService navigationService, MediaStore mediaStore, PlaylistStore playlistStore, PlaylistBrowserNavigationStore playlistBrowserNavigationStore)
        {
            _musicService = musicService;

            _playlistBrowserNavigationStore = playlistBrowserNavigationStore;

            _mediaStore = mediaStore;
            _playlistStore = playlistStore;

            RenamePlaylist = new RenamePlaylistAsyncCommand(_playlistStore, _playlistBrowserNavigationStore);
            ChangeBanner = new ChangeBannerAsyncCommand(_playlistStore, _playlistBrowserNavigationStore);

            _musicService.MusicPlayerEvent += OnMusicPlayerEvent;
            _mediaStore.PlaylistSongsAdded += OnPlaylistSongsAdded;
            _playlistStore.PlaylistBannerChanged += OnPlaylistBannerChange;

            PlaySong = new PlaySpecificSongCommand(musicService);

            OpenExplorer = new OpenExplorerAtPathCommand();

            CurrentPlaylistName = playlistStore.Playlists.FirstOrDefault(x => x.Id == playlistBrowserNavigationStore.BrowserPlaylistId)?.Name ?? "Undefined";

            CurrentDateString = DateTime.Now.ToString("dd MMM, yyyy");

            BannerUrl = playlistStore.Playlists.FirstOrDefault(x => x.Id == playlistBrowserNavigationStore.BrowserPlaylistId)?.Banner;
            
            PlaylistCreationDate = playlistStore.Playlists.FirstOrDefault(x => x.Id == playlistBrowserNavigationStore.BrowserPlaylistId)?.CreationDate?.ToString("dd MMM, yyyy") ?? DateTime.Now.ToString("dd MMM, yyyy");

            Task.Run(LoadSongs);
        }

        // TODO: Fix number hierarchy after DeleteSong called
        private void LoadSongs()
        {
            AllSongsOfPlaylist = new ObservableCollection<MediaModel>(_mediaStore.Songs.Where(x => x.PlayerlistId == _playlistBrowserNavigationStore.BrowserPlaylistId).Select((x, num) =>
            {
                return new MediaModel
                {
                    Playing = _musicService.PlayerState == PlaybackState.Playing && x.Id == _musicService.CurrentMedia?.Id,
                    Number = num + 1,
                    Id = x.Id,
                    Title = Path.GetFileNameWithoutExtension(x.FilePath),
                    Path = x.FilePath,
                    Duration = AudioUtills.DurationParse(x.FilePath)
                };
            }).ToList());
            DeleteSong = new DeleteSpecificSongAsyncCommand(_musicService, _mediaStore, AllSongsOfPlaylist);
            AddSong = new AddSongAsyncCommand(_musicService, _mediaStore, _playlistBrowserNavigationStore, AllSongsOfPlaylist);
            OnPropertyChanged(nameof(AllSongsOfPlaylist));

        }

        private void OnMusicPlayerEvent(object? sender, MusicPlayerEventArgs e)
        {
            switch (e.Type)
            {
                case PlayerEventType.Playing:
                    var songPlay = AllSongsOfPlaylist?.FirstOrDefault(x => x.Id == e.Media?.Id);
                    if (songPlay != null)
                    {
                        songPlay.Playing = true;
                    }
                    break;
                default:
                    var songStopped = AllSongsOfPlaylist?.FirstOrDefault(x => x.Id == e.Media?.Id);
                    if (songStopped != null)
                    {
                        songStopped.Playing = false;
                    }
                    break;
            }
        }

        private void OnPlaylistSongsAdded(object? sender, PlaylistSongsAddedEventArgs e)
        {
            foreach (MediaEntity mediaEntity in e.Songs)
            {
                if (mediaEntity.PlayerlistId == _playlistBrowserNavigationStore.BrowserPlaylistId)
                {
                    string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\songs" + "\\" + Path.GetFileName(mediaEntity.FilePath);
                    File.Copy(mediaEntity.FilePath, path);
                    var songsIndex = AllSongsOfPlaylist?.Count;
                    AllSongsOfPlaylist?.Add(new MediaModel
                    {
                        Playing = _musicService.PlayerState == PlaybackState.Playing && mediaEntity.Id == _musicService.CurrentMedia?.Id,
                        Number = songsIndex + 1,
                        Id = mediaEntity.Id,
                        Title = Path.GetFileNameWithoutExtension(path),
                        Path = path,
                        Duration = AudioUtills.DurationParse(path)
                    });
                }
            }
        }

        private void OnPlaylistBannerChange(object? sender, PlaylistBannerChangedArgs args)
        {
            BannerUrl = args.Banner;
        }

        public async Task OnFilesDroppedAsync(string[] files, object? parameter)
        {
            var mediaEntities = files.Where(x => PathExtension.HasAudioVideoExtensions(x)).Select(x => new MediaEntity
            {
                PlayerlistId = _playlistBrowserNavigationStore.BrowserPlaylistId,
                FilePath = x
            }).ToList();

            await _mediaStore.AddRange(mediaEntities);

            foreach (MediaEntity mediaEntity in mediaEntities)
            {
                var songsIndex = AllSongsOfPlaylist?.Count;
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "\\songs" + "\\" + Path.GetFileName(mediaEntity.FilePath);
                if (!File.Exists(path))
                {
                    File.Copy(mediaEntity?.FilePath, path);
                }
                AllSongsOfPlaylist?.Add(new MediaModel
                {
                    Playing = _musicService.PlayerState == PlaybackState.Playing && mediaEntity.Id == _musicService.CurrentMedia?.Id,
                    Number = songsIndex + 1,
                    Id = mediaEntity.Id,
                    Title = Path.GetFileNameWithoutExtension(path),
                    Path = path,
                    Duration = AudioUtills.DurationParse(path)
                });
            }
        }


        public override void Dispose()
        {
            _musicService.MusicPlayerEvent -= OnMusicPlayerEvent;
            _mediaStore.PlaylistSongsAdded -= OnPlaylistSongsAdded;
            _playlistStore.PlaylistBannerChanged -= OnPlaylistBannerChange;


        }
    }
}
