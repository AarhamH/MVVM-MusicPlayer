using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using dotnet_player_client.Models;
using dotnet_player_client.Observables;
using dotnet_player_client.Services;
using dotnet_player_client.ViewModels;

namespace dotnet_player_client.Command
{
    public class SearchYoutubeCommandAsync : BaseCommandAsync
    {
        private readonly IYouTubeClientService _youTubeClientService;
        private readonly ObservableCollection<YoutubeModel> _observableYoutube;
        private readonly DownloadVM _downloadVM;

        public SearchYoutubeCommandAsync(IYouTubeClientService youTubeClientService, ObservableCollection<YoutubeModel> observableYoutube, DownloadVM downloadVM)
        {
            _youTubeClientService = youTubeClientService;
            _observableYoutube = observableYoutube;
            _downloadVM = downloadVM;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            if(parameter is string searchText)
            {
                _observableYoutube.Clear();
                _downloadVM.IsLoadingSearch = true;
                List<YoutubeInfo>? videos = await _youTubeClientService.SearchVideoByName(searchText);

                if(videos == null)
                {
                    _downloadVM.IsLoadingSearch = false;
                    _downloadVM.IsFailedSearch = true;
                    return;
                }

                string[] downloadFiles = Array.Empty<string>();
                try
                {
                    downloadFiles = Directory.GetFiles("downloads\\").Select(x => _youTubeClientService.GetSafeFileName(Path.GetFileName(x))).ToArray();
                }
                catch { }

                var videomodels = videos.Select((x, num) => {
                    bool isDownloaded = downloadFiles.FirstOrDefault(y => y == _youTubeClientService.GetSafeFileName(x.TitleInfo + ".mp3")) != null;
                    return new YoutubeModel
                    {
                        IsDownloading = isDownloaded,
                        DownloadProgress = isDownloaded ? 100 : 0,
                        FinishedDownload = isDownloaded ? true : null,
                        Num = num + 1,
                        Title = x.TitleInfo,
                        URL = x.URLInfo,
                        Length = x.LengthInfo,
                        Channel = x.ChannelInfo,
                        Views = x.ViewsInfo
                    };
                });
                foreach(var video in videomodels)
                {
                    _observableYoutube.Add(video);
                }

                _downloadVM.IsFailedSearch = false;
                _downloadVM.IsLoadingSearch = false;
            }
        }
    }
}
    