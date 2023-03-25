using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.ViewModels
{
    public class MainVM : VMBase
    {
        public string BarTitle { get; } = "Audio Player";

        private VMBase? _currentView;
        public VMBase? CurrentView
        {
            get { return _currentView; }
            set
            {
                if (value == _currentView) return;
                _currentView?.Dispose();
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private VMBase? _playerView;
        public VMBase? PlayerView
        {
            get { return _playerView; }
            set
            {
                if (value == _playerView) return;
                _playerView?.Dispose();
                _playerView = value;
                OnPropertyChanged();
            }
        }

        private VMBase? _barView;
        public VMBase? BarView
        {
            get { return _barView; }
            set
            {
                if (value == _barView) return;
                _barView?.Dispose();
                _barView = value;
                OnPropertyChanged();
            }
        }

        private VMBase? _modalView;
        public VMBase? ModalView
        {
            get { return _modalView; }
            set
            {
                if (value == _modalView) return;
                _modalView?.Dispose();
                _modalView = value;
                OnPropertyChanged();
            }
        }

        public MainVM(HomeVM homeView, PlayerVM playerVM, ToolbarVM toolbardVM)
        {
            CurrentView = homeView;
            // create player view

        }
    }
}
