using dotnet_player_client.Enumeration;
using dotnet_player_client.Arguments;
using dotnet_player_client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerClient.ViewModels;

namespace dotnet_player_client.Services
{
    public interface INavigationService
    {
        public event EventHandler<PageChangeArgs> PageChangedEvent;
        public PageType CurrentPage { get; }
        public void NavigateHome();
        public void NavigatePlaylist();
        public void NavigateDownloads();
    }

    public class NavigationService: INavigationService
    {
        private readonly Func<MainVM>? _mainViewModelFunc;
        private readonly Func<HomeVM>? _homeViewModelFunc;
        private readonly Func<PlayListVM>? _playlistViewModelFunc;
        private readonly Func<DownloadVM>? _downloadViewModelFunc;

        public event EventHandler<PageChangeArgs>? PageChangedEvent;
        public PageType CurrentPage { get; private set; } = PageType.Home;

        public NavigationService(Func<MainVM> mainViewModelFunc, Func<HomeVM> homeViewModelFunc,
                                 Func<PlayListVM> playlistViewModelFunc, Func<DownloadVM> downloadViewModelFunc)
        {
            _mainViewModelFunc = mainViewModelFunc;
            _homeViewModelFunc = homeViewModelFunc;
            _playlistViewModelFunc = playlistViewModelFunc;
            _downloadViewModelFunc = downloadViewModelFunc;
        }

        public void NavigateHome()
        {
            var mainVm = _mainViewModelFunc?.Invoke();
            var homeVm = _homeViewModelFunc?.Invoke();

            if (mainVm != null && mainVm.CurrentView is not HomeVM)
            {
                mainVm.CurrentView = homeVm;
                CurrentPage = PageType.Home;
                PageChangedEvent?.Invoke(this, new PageChangeArgs(CurrentPage));
            }
        }

        public void NavigatePlaylist()
        {
            var mainVm = _mainViewModelFunc?.Invoke();
            var playlistVm = _playlistViewModelFunc?.Invoke();

            if (mainVm != null)
            {
                mainVm.CurrentView = playlistVm;
                CurrentPage = PageType.Playlist;
                PageChangedEvent?.Invoke(this, new PageChangeArgs(CurrentPage));
                
            }
        }

        public void NavigateDownloads()
        {
            var mainVm = _mainViewModelFunc?.Invoke();
            var downloadsVm = _downloadViewModelFunc?.Invoke();

            if (mainVm != null && mainVm.CurrentView is not DownloadVM)
            {
                mainVm.CurrentView = downloadsVm;
                CurrentPage = PageType.Download;
                PageChangedEvent?.Invoke(this, new PageChangeArgs(CurrentPage));
            }
        }
    }
}
