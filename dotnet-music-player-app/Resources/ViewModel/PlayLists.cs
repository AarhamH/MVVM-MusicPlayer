using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnet_music_player_app.Resources.Model;

namespace dotnet_music_player_app.Resources.ViewModel
{
    class PlayLists : Utils.ViewModelBase
    {
        private readonly PagesModule _pagesModule;
        public int SongNumber
        {
            get { return _pagesModule.SongNum; }
            set { _pagesModule.SongNum = value; OnPropertyChanged(); }
        }

        public PlayLists()
        {
            _pagesModule = new PagesModule();
            SongNumber = 0;
        }
    }
}