using dotnet_player_client.Services;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using dotnet_player_client.Command;
using NAudio.Wave;
using dotnet_player_client.Arguments;
using dotnet_player_client.Observables;
using dotnet_player_client.Utilities;
using dotnet_player_client.Enumeration;
using dotnet_player_client.ViewModels;

namespace dotnet_player_client.ViewModels
{
    public class PlayerVM : VMBase
    {
        private readonly IPlayerService _musicService;

        private bool _playNext;

        public int Volume
        {
            get => (int)Math.Ceiling(_musicService.Volume * 100);
            set
            {
                _musicService.Volume = value / 100f;
                OnPropertyChanged();
            }
        }

        private string? _songName;
        public string? SongName
        {
            get => _songName;
            set
            {
                _songName = value;
                OnPropertyChanged();
            }
        }

        private string? _songPath;
        public string? SongPath
        {
            get => _songPath;
            set
            {
                _songPath = value;
                OnPropertyChanged();
            }
        }

        public long SongProgress
        {
            get => _musicService.Position;
            set
            {
                _musicService.Position = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SongProgressFormatted));
            }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }

        public string SongProgressFormatted => AudioUtill.DurationParse(SongProgress);
        public long SongDuration => _musicService.TotalTime;

        public string SongDurationFormatted => AudioUtill.DurationParse(SongDuration);

        public ICommand TogglePlayer { get; }
        public ICommand PlayBackward { get; }
        public ICommand PlayForward { get; }
        public ICommand OpenExplorer { get; }
        public ICommand ToggleVolume { get; }

        public PlayerVM(IPlayerService musicService)
        {
            _musicService = musicService;
            PlayBackward = new GoBackwardCommand(musicService);
            PlayForward = new GoForwardCommand(musicService);
            TogglePlayer = new TogglePlayerCommand(musicService);
            OpenExplorer = new OpenExplorerCommand();
            ToggleVolume = new ToggleVolumeCommand(this);

            _musicService.MusicPlayerEvent += OnMusicPlayerEvent;
            _musicService.AfterMusicPlayerEvent += OnAfterMusicPlayerEvent;

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(SongProgress));
            OnPropertyChanged(nameof(SongDuration));
            OnPropertyChanged(nameof(SongProgressFormatted));
            OnPropertyChanged(nameof(SongDurationFormatted));
        }

        private void OnMusicPlayerEvent(object? sender, SongArgs e)
        {
            _playNext = false;
            switch (e.FuncType)
            {
                case PlayerFuncType.Playing:
                    IsPlaying = true;
                    break;
                case PlayerFuncType.Finished:
                    IsPlaying = false;
                    _playNext = true;
                    break;
                default:
                    IsPlaying = false;
                    break;
            }

            SongName = _musicService.PlayingSongName;
            SongPath = _musicService.PlayingSongPath;
        }

        private void OnAfterMusicPlayerEvent(object? sender, EventArgs args)
        {
            if (_playNext)
            {
                _musicService.PlayNext(false);
                _playNext = false;
            }
        }

        public override void Dispose()
        {
            _musicService.MusicPlayerEvent -= OnMusicPlayerEvent;
            _musicService.AfterMusicPlayerEvent -= OnAfterMusicPlayerEvent;
        }
    }
}
