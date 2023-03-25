using MusicPlayerClient.Enums;
using MusicPlayerClient.Events;
using MusicPlayerClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerClient.Services
{
    public interface INavigationService
    {
        public event EventHandler<PageChangedEventArgs> PageChangedEvent;
        public PageType CurrentPage { get; }
        public void NavigateHome();
        public void NavigatePlaylist();
        public void NavigateDownloads();
    }

    public class NavigationService: INavigationService
    {
        private readonly Func<MainViewModel>? _mainViewModelFunc;
        private readonly Func<HomeViewModel>? _homeViewModelFunc;
        private readonly Func<PlaylistViewModel>? _playlistViewModelFunc;
        private readonly Func<DownloadsViewModel>? _downloadViewModelFunc;

        public event EventHandler<PageChangedEventArgs>? PageChangedEvent;
        public PageType CurrentPage { get; private set; } = PageType.Home;

        public NavigationService(Func<MainViewModel> mainViewModelFunc, Func<HomeViewModel> homeViewModelFunc,
                                 Func<PlaylistViewModel> playlistViewModelFunc, Func<DownloadsViewModel> downloadViewModelFunc)
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

            if (mainVm != null && mainVm.CurrentView is not HomeViewModel)
            {
                mainVm.CurrentView = homeVm;
                CurrentPage = PageType.Home;
                PageChangedEvent?.Invoke(this, new PageChangedEventArgs(CurrentPage));
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
                PageChangedEvent?.Invoke(this, new PageChangedEventArgs(CurrentPage));
            }
        }

        public void NavigateDownloads()
        {
            var mainVm = _mainViewModelFunc?.Invoke();
            var downloadsVm = _downloadViewModelFunc?.Invoke();

            if (mainVm != null && mainVm.CurrentView is not DownloadsViewModel)
            {
                mainVm.CurrentView = downloadsVm;
                CurrentPage = PageType.Downloads;
                PageChangedEvent?.Invoke(this, new PageChangedEventArgs(CurrentPage));
            }
        }
    }
}
