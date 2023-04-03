using Microsoft.EntityFrameworkCore;
using dotnet_player_client.Commands;
using dotnet_player_client.Services;
using dotnet_player_data.Data;
using dotnet_player_data.DataEntities;
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
using dotnet_player_client.Events;
using System.Diagnostics;
using dotnet_player_client.Stores;
using dotnet_player_client.Interfaces;
using System.Windows;
using dotnet_player_client.Extensions;
using dotnet_player_client.Core;
using dotnet_player_client.Enums;

namespace dotnet_player_client.ViewModels
{
    public class HomeViewModel : ViewModelBase, IFilesDropAsync
    {
        private readonly IMusicPlayerService _musicService;
        private readonly MediaStore _mediaStore;
        public string CurrentDateString { get; }
        public ObservableCollection<MediaModel>? AllSongs { get; set; }
        public ICommand PlaySong { get; } 
        public ICommand OpenExplorer { get; } 
        public ICommand? DeleteSong { get; set; }

        public HomeViewModel(IDbContextFactory<DataContext> dbContextFactory, MediaStore mediaStore, IMusicPlayerService musicService)
        {
            _musicService = musicService;

            _mediaStore = mediaStore;

            _musicService.MusicPlayerEvent += OnMusicPlayerEvent;
            _mediaStore.PlaylistSongsAdded += OnPlaylistSongsAdded;

            PlaySong = new PlaySpecificSongCommand(musicService);

            OpenExplorer = new OpenExplorerAtPathCommand();

            CurrentDateString = DateTime.Now.ToString("dd MMM, yyyy");

            Task.Run(LoadSongs);
        }

        private void LoadSongs()
        {
            AllSongs = new ObservableCollection<MediaModel>(_mediaStore.Songs.Where(x => x.PlayerlistId == null).Select((x, num) =>
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

            OnPropertyChanged(nameof(AllSongs));

            DeleteSong = new DeleteSpecificSongAsyncCommand(_musicService, _mediaStore, AllSongs);
        }

        private void OnMusicPlayerEvent(object? sender, MusicPlayerEventArgs e)
        {
            switch (e.Type)
            {
                case PlayerEventType.Playing:
                    var songPlay = AllSongs?.FirstOrDefault(x => x.Id == e.Media?.Id);
                    if(songPlay != null)
                    {
                        songPlay.Playing = true;
                    }
                    break;
                default:
                    var songStopped = AllSongs?.FirstOrDefault(x => x.Id == e.Media?.Id);
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
                if (mediaEntity.PlayerlistId is null)
                {
                    var songsIndex = AllSongs?.Count;
                    AllSongs?.Add(new MediaModel
                    {
                        Playing = _musicService.PlayerState == PlaybackState.Playing && mediaEntity.Id == _musicService.CurrentMedia?.Id,
                        Number = songsIndex + 1,
                        Id = mediaEntity.Id,
                        Title = Path.GetFileNameWithoutExtension(mediaEntity.FilePath),
                        Path = mediaEntity.FilePath,
                        Duration = AudioUtills.DurationParse(mediaEntity.FilePath)
                    });
                }
            }
        }

        public async Task OnFilesDroppedAsync(string[] files, object? parameter)
        {
            var mediaEntities = files.Where(x => PathExtension.HasAudioVideoExtensions(x)).Select(x => new MediaEntity
            {
                FilePath = x
            }).ToList();

            await _mediaStore.AddRange(mediaEntities);

            foreach (MediaEntity mediaEntity in mediaEntities)
            {
                var songsIndex = AllSongs?.Count;
                AllSongs?.Add(new MediaModel
                {
                    Playing = _musicService.PlayerState == PlaybackState.Playing && mediaEntity.Id == _musicService.CurrentMedia?.Id,
                    Number = songsIndex + 1,
                    Id = mediaEntity.Id,
                    Title = Path.GetFileNameWithoutExtension(mediaEntity.FilePath),
                    Path = mediaEntity.FilePath,
                    Duration = AudioUtills.DurationParse(mediaEntity.FilePath)
                });
            }
        }

        public override void Dispose()
        {
            _musicService.MusicPlayerEvent -= OnMusicPlayerEvent;
            _mediaStore.PlaylistSongsAdded -= OnPlaylistSongsAdded;
        }
    }
}
