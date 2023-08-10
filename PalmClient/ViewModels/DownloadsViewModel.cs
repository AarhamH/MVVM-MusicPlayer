using Microsoft.EntityFrameworkCore;
using PalmClient.Commands;
using PalmClient.Services;
using PalmData.Data;
using PalmData.DataEntities;
using PalmClient.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PalmClient.Events;
using System.Diagnostics;
using PalmClient.Stores;
using PalmClient.Interfaces;
using System.Windows;
using PalmClient.Extensions;
using PalmClient.Core;

namespace PalmClient.ViewModels
{
    public class DownloadsViewModel : ViewModelBase
    {
        private readonly IMusicPlayerService _musicService;
        private readonly MediaStore _mediaStore;
        public string CurrentDateString { get; }
        public ObservableCollection<YoutubeVideoInfoModel> ResultMedia { get; }

        private bool _isLoadingSearch;
        public bool IsLoadingSearch
        {
            get => _isLoadingSearch;
            set
            {
                _isLoadingSearch = value;
                OnPropertyChanged();
            }
        }

        private bool _isFailedSearch;
        public bool IsFailedSearch
        {
            get => _isFailedSearch;
            set
            {
                _isFailedSearch = value;
                OnPropertyChanged();
            }
        }

        public ICommand? SearchMedia { get; set; }
        public ICommand DownloadMedia { get; }

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        
        public DownloadsViewModel(MediaStore mediaStore, IMusicPlayerService musicService, IYouTubeClientService youtubeClient, INavigationService navigationService, PlaylistBrowserNavigationStore playlistBrowserNavigationStore)
        {
            _musicService = musicService;

            ResultMedia = new ObservableCollection<YoutubeVideoInfoModel>();

            SearchMedia = new SearchSongOnYoutubeAsyncCommand(youtubeClient, ResultMedia, this);

            DownloadMedia = new DownloadSongOnYoutubeAsyncCommand(youtubeClient, ResultMedia);

            _mediaStore = mediaStore;

            CurrentDateString = DateTime.Now.ToString("dd MMM, yyyy");
        }

    }
}
