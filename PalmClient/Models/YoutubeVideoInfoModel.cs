using PalmClient.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Models
{
    public class YoutubeVideoInfo
    {
        public string? Title { get; set; }
        public string? Channel { get; set; }
        public string? Views { get; set; }
        public string? Url { get; set; }
        public string? Duration { get; set; }

        public string? Thumbnail { get; set; }
    }

    public class YoutubeVideoInfoModel : ObservableObject
    {
        private int _num;
        private bool _downloading;
        private bool? _finishedDownload;
        private int _downloadProgress;
        private string? _channel;
        private string? _views;
        private string? _title;
        private string? _url;
        private string? _duration;
        private string? _thumbnail;

        public int Num
        {
            get { return _num; }
            set
            {
                if (_num != value)
                {
                    _num = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Downloading
        {
            get { return _downloading; }
            set
            {
                if (_downloading != value)
                {
                    _downloading = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool? FinishedDownload
        {
            get { return _finishedDownload; }
            set
            {
                if (_finishedDownload != value)
                {
                    _finishedDownload = value;
                    OnPropertyChanged();
                }
            }
        }

        public int DownloadProgress
        {
            get { return _downloadProgress; }
            set
            {
                if (_downloadProgress != value)
                {
                    _downloadProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Channel
        {
            get { return _channel; }
            set
            {
                if (_channel != value)
                {
                    _channel = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Views
        {
            get { return _views; }
            set
            {
                if (_views != value)
                {
                    _views = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Url
        {
            get { return _url; }
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Thumbnail
        {
            get { return _thumbnail; }
            set
            {
                if (_thumbnail != value)
                {
                    _thumbnail = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
