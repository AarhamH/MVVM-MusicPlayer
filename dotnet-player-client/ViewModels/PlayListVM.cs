using dotnet_player_client.Command;
using dotnet_player_client.Services;
using dotnet_player_client.Utilities;
using dotnet_player_client.Observables;
using dotnet_player_client.Enumeration;
using dotnet_player_client.Arguments;
using dotnet_player_client.FileDropInterface;
using dotnet_player_client.Models;
using dotnet_player_client.Stores;
using dotnet_player_data.Objects;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using dotnet_player_client.ViewModels;

namespace dotnet_player_client.ViewModels
{
    public class PlayListVM : VMBase
    {
        private readonly IPlayerService _musicService;
        private readonly BrowserNavStorage _playlistBrowserNavigationStore;
        private readonly SongStorage _mediaStore;
        private readonly PLStorage _playlistStore;

        public string? _currentPlaylistName;
        public string? CurrentPlaylistName
        {
            get => _currentPlaylistName;
            set
            {
                _currentPlaylistName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SongModel>? AllSongsOfPlaylist { get; set; }
        public ICommand RenamePlaylist { get; }
        public ICommand PlaySong { get; }
        public ICommand OpenExplorer { get; }
        public ICommand? DeleteSong { get; set; }

        public PlayListVM(IPlayerService musicService, INavigationService navigationService, SongStorage mediaStore, PLStorage playlistStore, BrowserNavStorage playlistBrowserNavigationStore)
        {
            _musicService = musicService;

            _playlistBrowserNavigationStore = playlistBrowserNavigationStore;

            _mediaStore = mediaStore;
            _playlistStore = playlistStore;

            RenamePlaylist = new RenamePlaylistCommandAsync(_playlistStore, _playlistBrowserNavigationStore);
            
            _musicService.MusicPlayerEvent += OnMusicPlayerEvent;
            _mediaStore.PLAppended += OnPlaylistSongsAdded;

            PlaySong = new PlaySongCommand(musicService);

            OpenExplorer = new OpenExplorerCommand();

            _currentPlaylistName = playlistStore.PlayList.FirstOrDefault(x => x.Id == playlistBrowserNavigationStore.BrowserPlaylistID)?.PLTitle ?? "Undefined";


            Task.Run(LoadSongs);
        }

        private void LoadSongs()
        {
            AllSongsOfPlaylist = new ObservableCollection<SongModel>(_mediaStore.Songs.Where(x => x.ListID == _playlistBrowserNavigationStore.BrowserPlaylistID).Select((x, num) =>
            {
                return new SongModel
                {
                    Playing = _musicService.PlayerState == PlaybackState.Playing && x.Id == _musicService.CurrentSong?.Id,
                    Number = num + 1,
                    Id = x.Id,
                    Title = Path.GetFileNameWithoutExtension(x.Path),
                    Path = x.Path,
                    Duration = AudioUtill.DurationParse(x.Path)
                };
            }).ToList());

            OnPropertyChanged(nameof(AllSongsOfPlaylist));

            DeleteSong = new DeleteSongCommandAsync(_musicService, _mediaStore, AllSongsOfPlaylist);
        }
        private void OnMusicPlayerEvent(object? sender, SongArgs e)
        {
            switch (e.FuncType)
            {
                case PlayerFuncType.Playing:
                    var songPlay = AllSongsOfPlaylist?.FirstOrDefault(x => x.Id == e.SongObj?.Id);
                    if (songPlay != null)
                    {
                        songPlay.Playing = true;
                    }
                    break;
                default:
                    var songStopped = AllSongsOfPlaylist?.FirstOrDefault(x => x.Id == e.SongObj?.Id);
                    if (songStopped != null)
                    {
                        songStopped.Playing = false;
                    }
                    break;
            }
        }

        private void OnPlaylistSongsAdded(object? sender, PLAppendArgs e)
        {
            foreach (SongObjects mediaEntity in e.Songs)
            {
                if (mediaEntity.ListID == _playlistBrowserNavigationStore.BrowserPlaylistID)
                {
                    var songsIndex = AllSongsOfPlaylist?.Count;
                    AllSongsOfPlaylist?.Add(new SongModel
                    {
                        Playing = _musicService.PlayerState == PlaybackState.Playing && mediaEntity.Id == _musicService.CurrentSong?.Id,
                        Number = songsIndex + 1,
                        Id = mediaEntity.Id,
                        Title = Path.GetFileNameWithoutExtension(mediaEntity.Path),
                        Path = mediaEntity.Path,
                        Duration = AudioUtill.DurationParse(mediaEntity.Path)
                    });
                }
            }
        }

        public async Task OnFilesDroppedAsync(string[] files, object? parameter)
        {
            var mediaEntities = files.Where(x => PathUtil.HasAudioVideoExtensions(x)).Select(x => new SongObjects
            {
                ListID = _playlistBrowserNavigationStore.BrowserPlaylistID,
                Path = x
            }).ToList();

            await _mediaStore.AppendRange(mediaEntities);

            foreach (SongObjects mediaEntity in mediaEntities)
            {
                var songsIndex = AllSongsOfPlaylist?.Count;
                AllSongsOfPlaylist?.Add(new SongModel
                {
                    Playing = _musicService.PlayerState == PlaybackState.Playing && mediaEntity.Id == _musicService.CurrentSong?.Id,
                    Number = songsIndex + 1,
                    Id = mediaEntity.Id,
                    Title = Path.GetFileNameWithoutExtension(mediaEntity.Path),
                    Path = mediaEntity.Path,
                    Duration = AudioUtill.DurationParse(mediaEntity.Path)
                });
            }
        }

        public override void Dispose()
        {
            _musicService.MusicPlayerEvent -= OnMusicPlayerEvent;
            _mediaStore.PLAppended -= OnPlaylistSongsAdded; 
        }
    }
}
