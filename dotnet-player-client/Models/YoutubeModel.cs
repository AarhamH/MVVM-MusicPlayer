using dotnet_player_client.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Models
{
    public class YoutubeInfo
    {
        public string? TitleInfo {  get; set; }
        public string? ChannelInfo { get; set;}

        public string? ViewsInfo { get; set; }
        public string? URLInfo { get; set; }
        public string? LengthInfo { get; set; }

    }

    public class YoutubeModel: ObsObject
    {
        private int vid_num;
        private bool vid_isDownloading;
        private bool? vid_isFinishedDownload;
        private int? vid_downloadProgress;
        private string? vid_channel;
        private string? vid_views;
        private string? vid_title;
        private string? vid_url;
        private string? vid_length;

        public int Num
        {
            get { return vid_num; }
            set
            {
                if(vid_num != value)
                {
                    vid_num = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDownloading
        {
            get { return vid_isDownloading; }
            set
            {
                if (vid_isDownloading != value)
                {
                    vid_isDownloading = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool? FinishedDownload
        {
            get { return vid_isFinishedDownload; }
            set
            {
                if (vid_isFinishedDownload != value)
                {
                    vid_isFinishedDownload = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? DownloadProgress
        {
            get { return vid_downloadProgress; }
            set
            {
                if (vid_downloadProgress != value)
                {
                    vid_downloadProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Channel
        {
            get { return vid_channel; }
            set
            {
                if(vid_channel != value)
                {
                    vid_channel = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Views
        {
            get { return vid_views; }
            set
            {
                if (vid_views != value)
                {
                    vid_views = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Title
        {
            get { return vid_title; }
            set
            {
                if (vid_title != value)
                {
                    vid_title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? URL
        {
            get { return vid_url; }
            set
            {
                if (vid_url != value)
                {
                    vid_url = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Length
        {
            get { return vid_length; }
            set
            {
                if (vid_length != value)
                {
                    vid_length = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
