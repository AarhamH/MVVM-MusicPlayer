using dotnet_player_client.Extensions;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace dotnet_player_client.Commands
{
    public class AddSongAsyncCommand : AsyncCommandBase
    {
        private readonly IMusicPlayerService _musicService;
        private readonly MediaStore _mediaStore;
        private readonly ObservableCollection<MediaModel>? _observableSongs;

        public AddSongAsyncCommand(IMusicPlayerService musicService, MediaStore mediaStore)
        {
            _musicService = musicService;
            _mediaStore = mediaStore;
        }

        public AddSongAsyncCommand(IMusicPlayerService musicService, MediaStore mediaStore, ObservableCollection<MediaModel> observableSong) : this(musicService, mediaStore)
        {
            _observableSongs = observableSong;
        }

        protected override Task ExecuteAsync(object? parameter)
        {
            throw new NotImplementedException();
        }

    }
}
