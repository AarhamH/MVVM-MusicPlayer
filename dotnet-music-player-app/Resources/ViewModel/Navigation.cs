using dotnet_music_player_app.Resources.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dotnet_music_player_app.Resources.ViewModel
{
    class Navigation : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged();  }
        }

        public ICommand SongPageCommand { get; set; }
        public ICommand PlaylistCommand { get; set; }
        public ICommand MostPlayedCommand { get; set; }

        private void SongPage(object obj) => CurrentView = new SongPage();
        private void PlayList(object obj) => CurrentView = new PlayLists();
        private void MostPlayed(object obj) => CurrentView = new MostPlayed();

        public Navigation()
        {
            SongPageCommand = new Relay(SongPage);
            PlaylistCommand = new Relay(PlayList);
            MostPlayedCommand = new Relay(MostPlayed);

            CurrentView = new SongPage();
        }
    }
}
