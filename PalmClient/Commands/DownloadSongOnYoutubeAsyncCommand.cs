using PalmClient.Models;
using PalmClient.Core;
using PalmClient.Services;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PalmClient.Commands
{
    public class DownloadSongOnYoutubeAsyncCommand : AsyncCommandBase
    {
        private readonly IYouTubeClientService _youtubeClient;
        private readonly ObservableCollection<YoutubeVideoInfoModel> _observableMedia;

        public DownloadSongOnYoutubeAsyncCommand(IYouTubeClientService youtubeClient, ObservableCollection<YoutubeVideoInfoModel> observableMedia)
        {
            _youtubeClient = youtubeClient;
            _observableMedia = observableMedia;

            PreventClicksWhileExecuting = false;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            if(parameter is string url)
            {
                var dir = Directory.CreateDirectory("downloads\\");

                YoutubeVideoInfoModel? video = _observableMedia.FirstOrDefault(x => x.Url == url);
                var fileName = dir.FullName + video?.Title + ".mp3";
                if (video != null && !video.Downloading) 
                {
                    try
                    {
                        video.FinishedDownload = false;
                        video.Downloading = true;

                        var download = _youtubeClient.DownloadYoutubeAudioAsync(video.Url!, fileName);
                        await foreach (var progress in download)
                        {
                            video.DownloadProgress = progress;
                        }

                        video.FinishedDownload = true;
                    }
                    catch
                    {
                        video.FinishedDownload = default;
                        video.Downloading = default;
                        video.DownloadProgress = default;
                        File.Delete(fileName);
                    }
                }
                else if(video != null && video.FinishedDownload == true)
                {
                    string argument = "/select, \"" + fileName + "\"";

                    Process.Start("explorer.exe", argument);
                }
            }
        }
    }
}
