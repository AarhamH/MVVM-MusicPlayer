using dotnet_player_client.Extensions;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_data.DataEntities;
using dotnet_player_client.Extensions;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace dotnet_player_client.Commands
{
    public class AddSongAsyncCommand : AsyncCommandBase
    {
        private readonly IMusicPlayerService _musicService;
        private readonly MediaStore _mediaStore;
        private readonly PlaylistBrowserNavigationStore _playlistBrowserNavigationStore;
        private readonly ObservableCollection<MediaModel>? _observableSongs;

        public AddSongAsyncCommand(IMusicPlayerService musicService, MediaStore mediaStore, PlaylistBrowserNavigationStore playlistBrowserNavigationStore)
        {
            _mediaStore = mediaStore;
            _musicService = musicService;
            _playlistBrowserNavigationStore = playlistBrowserNavigationStore;
        }
        public AddSongAsyncCommand(IMusicPlayerService musicService, MediaStore mediaStore, PlaylistBrowserNavigationStore playlistBrowserNavigationStore, ObservableCollection<MediaModel> observableSongs): this(musicService,mediaStore,playlistBrowserNavigationStore)
        {
            _observableSongs = observableSongs;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            var openFileDialog = new OpenFileDialog();

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string fileName = openFileDialog.FileName;
                
                var songEntity = new MediaEntity
                {
                    PlayerlistId = _playlistBrowserNavigationStore.BrowserPlaylistId,
                    FilePath = fileName,
                };

                await _mediaStore.Add(songEntity);

                _observableSongs?.Insert(_observableSongs.Count, new MediaModel
                {
                    Playing = false,
                    Id = songEntity.Id,
                    Title = Path.GetFileNameWithoutExtension(fileName),
                    Path = fileName,
                    Duration = AudioUtills.DurationParse(fileName),
                    Number = _observableSongs?.Count+1
                });
                
            }
        }

    }
}
