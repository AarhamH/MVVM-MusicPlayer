using dotnet_player_client.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Models
{
    public class SongModel : ObsObject
    {
        private bool isPlaying;
        private int? songNum;
        private int? songID;
        private string? songTitle;
        private string? songPath;
        private string? songDuration;

        public bool Playing
        {
            get { return isPlaying; }
            set
            {
                if(isPlaying != value)
                {
                    isPlaying = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? Number
        {
            get { return songNum; }
            set
            {
                if (songNum != value)
                {
                    songNum = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? Id
        {
            get { return songID; }
            set
            {
                if (songID != value)
                {
                    songID = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Title
        {
            get { return songTitle; }
            set
            {
                if (songTitle != value)
                {
                    songTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Path
        {
            get { return songPath; }
            set
            {
                if (songPath != value)
                {
                    songPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Duration
        {
            get { return songDuration; }
            set
            {
                if (songDuration != value)
                {
                    songDuration = value;
                    OnPropertyChanged();
                }
            }
        }



    }
}
