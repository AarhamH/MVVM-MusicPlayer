using PalmClient.Extensions;
using PalmClient.Models;
using PalmClient.Services;
using PalmClient.Stores;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Commands
{
    public class DeleteSpecificPlaylistAsyncCommand : AsyncCommandBase
    {
        private readonly IMusicPlayerService _musicService;
        private readonly PlaylistStore _playlistStore;
        private readonly MediaStore _mediaStore;
        private readonly INavigationService _navigationService;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserStore;
        private readonly ObservableCollection<PlaylistModel>? _observablePlaylists;
        public DeleteSpecificPlaylistAsyncCommand(IMusicPlayerService musicService, INavigationService navigationService, PlaylistBrowserNavigationStore playlistBrowserStore, PlaylistStore playlistStore, MediaStore mediaStore)
        {
            _musicService = musicService;
            _playlistStore = playlistStore;
            _navigationService = navigationService;
            _playlistBrowserStore = playlistBrowserStore;
            _mediaStore = mediaStore;
        }

        public DeleteSpecificPlaylistAsyncCommand(IMusicPlayerService musicService, INavigationService navigationService, PlaylistBrowserNavigationStore playlistBrowserStore,
                                             PlaylistStore playlistStore, MediaStore mediaStore, ObservableCollection<PlaylistModel> observablePlaylists) : this(musicService, navigationService, playlistBrowserStore, playlistStore, mediaStore)
        {
            _observablePlaylists = observablePlaylists;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is int playlistId)
            {
                if (_musicService.CurrentMedia?.PlayerlistId == playlistId)
                {
                    _musicService.Stop();
                }

                _observablePlaylists?.RemoveAll(x => x.Id == playlistId);

                await _mediaStore.RemoveAll(x => x.PlayerlistId == playlistId);

                await _playlistStore.Remove(playlistId);

                if (_playlistBrowserStore.BrowserPlaylistId == playlistId)
                {
                    _navigationService.NavigateHome();
                }
            }
        }
    }
}
