using Microsoft.EntityFrameworkCore;
using dotnet_player_client.Command;
using dotnet_player_client.Services;
using dotnet_player_data.Context;
using dotnet_player_data.Objects;
using dotnet_player_client.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using dotnet_player_client.Arguments;
using System.Diagnostics;
using dotnet_player_client.Stores;
using dotnet_player_client.FileDropInterface;
using System.Windows;
using MusicPlayerClient.Extensions;
using dotnet_player_client.Observables;
using dotnet_player_client.Enumeration;
using dotnet_player_client.ViewModels;
using dotnet_player_client.Utilities;
using MusicPlayerClient.Commands;

namespace MusicPlayerClient.ViewModels
{
    public class HomeVM : VMBase
    {
        private readonly IPlayerService _musicService;
        private readonly SongStorage _mediaStore;
        public string CurrentDateString { get; }
        public ObservableCollection<SongModel>? AllSongs { get; set; }
        public ICommand PlaySong { get; }
        public ICommand OpenExplorer { get; }
        public ICommand? DeleteSong { get; set; }

        public HomeVM(IDbContextFactory<DataContext> dbContextFactory, MediaStore mediaStore, IMusicPlayerService musicService)
        {
            _musicService = musicService;

            _mediaStore = mediaStore;

            _musicService.MusicPlayerEvent += OnMusicPlayerEvent;
            _mediaStore.PlaylistSongsAdded += OnPlaylistSongsAdded;

            PlaySong = new PlaySpecificSongCommand(musicService);

            OpenExplorer = new OpenExplorerCommand();

            CurrentDateString = DateTime.Now.ToString("dd MMM, yyyy");

            Task.Run(LoadSongs);
        }

        private void LoadSongs()
        {
            AllSongs = new ObservableCollection<SongModel>(_mediaStore.Songs.Where(x => x.ListID == null).Select((x, num) =>
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

            OnPropertyChanged(nameof(AllSongs));

            DeleteSong = new DeleteSpecificSongAsyncCommand(_musicService, _mediaStore, AllSongs);
        }

        private void OnMusicPlayerEvent(object? sender, SongArgs e)
        {
            switch (e.FuncType)
            {
                case PlayerFuncType.Playing:
                    var songPlay = AllSongs?.FirstOrDefault(x => x.Id == e.SongObj?.Id);
                    if (songPlay != null)
                    {
                        songPlay.Playing = true;
                    }
                    break;
                default:
                    var songStopped = AllSongs?.FirstOrDefault(x => x.Id == e.SongObj?.Id);
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
                if (mediaEntity.ListID is null)
                {
                    var songsIndex = AllSongs?.Count;
                    AllSongs?.Add(new SongModel
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
                Path = x
            }).ToList();

            await _mediaStore.AppendRange(mediaEntities);

            foreach (SongObjects mediaEntity in mediaEntities)
            {
                var songsIndex = AllSongs?.Count;
                AllSongs?.Add(new SongModel
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
