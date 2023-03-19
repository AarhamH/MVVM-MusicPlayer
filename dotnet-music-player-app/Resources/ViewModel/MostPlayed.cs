using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnet_music_player_app.Resources.Model;

namespace dotnet_music_player_app.Resources.ViewModel
{
    class MostPlayed : Utils.ViewModelBase
    {
        private readonly PagesModule _pagesModule;
        public int SongNumber
        {
            get { return _pagesModule.SongNum; }
            set { _pagesModule.SongNum = value; OnPropertyChanged(); }
        }

        public string SongName
        {
            get { return _pagesModule.SongName; }
            set { _pagesModule.SongName = value; OnPropertyChanged(); }
        }

        public float SongTime
        {
            get { return _pagesModule.SongTime; }
            set { _pagesModule.SongTime = value; OnPropertyChanged(); }
        }

        public MostPlayed()
        {
            _pagesModule = new PagesModule();
            SongNumber = 0;
            SongName = "Song 1";
            SongTime = 1.5f;
        }
    }
}