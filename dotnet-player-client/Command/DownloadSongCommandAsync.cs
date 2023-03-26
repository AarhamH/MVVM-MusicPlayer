using dotnet_player_client.Models;
using dotnet_player_client.Services;
using dotnet_player_client.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class DownloadSongCommandAsync : BaseCommandAsync
    {
        private readonly IYouTubeClientService _youTubeClientService;
        private readonly ObservableCollection<YoutubeModel> _observableYoutube;

        public DownloadSongCommandAsync(IYouTubeClientService youTubeClientService, ObservableCollection<YoutubeModel> observableYoutube)
        {
            _youTubeClientService = youTubeClientService;
            _observableYoutube = observableYoutube;

            PreventClicksWhileExecuting = false;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is string url)
            {
                var dir = Directory.CreateDirectory("downloads\\");

                YoutubeModel? vid = _observableYoutube.FirstOrDefault(x => x.URL == url);
                var fileName = dir.FullName + vid?.Title + ".mp3";
                if (vid != null && !vid.IsDownloading)
                {
                    try
                    {
                        vid.FinishedDownload = false;
                        vid.IsDownloading = true;

                        var download = _youTubeClientService.DownloadYoutubeAudioAsync(vid.URL!, fileName);
                        await foreach (var progress in download)
                        {
                            vid.DownloadProgress = progress;
                        }
                        vid.FinishedDownload = true;
                    }

                    catch
                    {
                        vid.FinishedDownload = default;
                        vid.IsDownloading = default;
                        vid.DownloadProgress = default;
                        File.Delete(fileName);
                    }
                }
                else if(vid != null && vid.FinishedDownload == true)
                {
                    string arg = "/select, \"" + fileName + "\"";
                    Process.Start("explorer.exe", arg);
                }
            }
        }
    }
}
