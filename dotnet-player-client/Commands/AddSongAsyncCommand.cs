using dotnet_player_client.Extensions;
using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Stores;
using dotnet_player_data.DataEntities;
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
using System.Reflection;

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
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\songs"+"\\"+Path.GetFileName(fileName);

                var songEntity = new MediaEntity
                {
                    PlayerlistId = _playlistBrowserNavigationStore.BrowserPlaylistId,
                    FilePath = path,
                };


                if (!File.Exists(path))
                {
                    File.Copy(fileName, path);
                }

                await _mediaStore.Add(songEntity);

                _observableSongs?.Insert(_observableSongs.Count, new MediaModel
                {
                    Playing = false,
                    Id = songEntity.Id,
                    Title = Path.GetFileNameWithoutExtension(path),
                    Path = path,
                    Duration = AudioUtills.DurationParse(path),
                    Number = _observableSongs?.Count+1
                });
                
            }
        }

    }
}
