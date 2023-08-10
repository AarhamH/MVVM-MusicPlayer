using PalmClient.Models;
using PalmClient.Core;
using PalmClient.Services;
using PalmClient.ViewModels;
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
    public class SearchSongOnYoutubeAsyncCommand : AsyncCommandBase
    {
        private readonly IYouTubeClientService _youtubeClient;
        private readonly ObservableCollection<YoutubeVideoInfoModel> _observableMedia;
        private readonly DownloadsViewModel _downloads;
        public SearchSongOnYoutubeAsyncCommand(IYouTubeClientService youtubeClient, ObservableCollection<YoutubeVideoInfoModel> observableMedia, DownloadsViewModel downloads)
        {
            _youtubeClient = youtubeClient;
            _observableMedia = observableMedia;
            _downloads = downloads;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            if(parameter is string searchText)
            {
                _observableMedia.Clear();

                    _downloads.IsLoadingSearch = true;

                List<YoutubeVideoInfo>? videos = await _youtubeClient.SearchVideoByName(searchText);

                if(videos == null)
                {
                    _downloads.IsLoadingSearch = false;
                    _downloads.IsFailedSearch = true;
                    return;
                }
                string[] downloadedFiles = Array.Empty<string>();
                try
                {
                    downloadedFiles = Directory.GetFiles("downloads\\").Select(x => _youtubeClient.GetSafeFileName(Path.GetFileName(x))).ToArray();
                }
                catch { }

                var videomodels = videos.Select((x, num) => {
                    bool isDownloaded = downloadedFiles.FirstOrDefault(y => y == _youtubeClient.GetSafeFileName(x.Title + ".mp3")) != null;
                    return new YoutubeVideoInfoModel
                    {
                        Downloading = isDownloaded,
                        DownloadProgress = isDownloaded ? 100 : 0,
                        FinishedDownload = isDownloaded ? true : null,
                        Num = num + 1,
                        Title = x.Title,
                        Url = x.Url,
                        Duration = x.Duration,
                        Channel = x.Channel,
                        Views = x.Views,
                        Thumbnail = x.Thumbnail
                    };
                });

                foreach(var videomodel in videomodels)
                {
                    _observableMedia.Add(videomodel);
                }

                _downloads.IsFailedSearch = false;
                _downloads.IsLoadingSearch = false;
            }
        }
    }
}
