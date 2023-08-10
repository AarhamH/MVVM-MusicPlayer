using Microsoft.EntityFrameworkCore;
using PalmData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using PalmData.DataEntities;
using System.IO;
using System.Diagnostics;
using NAudio.Wave.SampleProviders;
using PalmClient.Events;
using PalmClient.Stores;
using PalmClient.Enums;

namespace PalmClient.Services
{
    public interface IMusicPlayerService
    {
        public event EventHandler<MusicPlayerEventArgs>? MusicPlayerEvent;
        public event EventHandler? AfterMusicPlayerEvent;
        public string PlayingSongPath { get; }
        public string PlayingSongName { get; }
        public MediaEntity? CurrentMedia { get; }
        public PlaybackState PlayerState { get; }
        public float Volume { get; set; }
        public long Position { get; set; }
        public long TotalTime { get; }
        public void Play(int mediaId);
        public void Stop();
        public void RePlay();
        public void PlayPause();
        public void PlayNext(bool callStoppedPlay = true);
        public void PlayPrevious();
    }

    public class MusicPlayerService : IMusicPlayerService
    {
        private readonly MediaStore _mediaStore;
        private IWavePlayer _waveOutDevice;
        private IWaveProvider? _audioFile;
        private MediaEntity? _currentMedia;
        public MediaEntity? CurrentMedia => _currentMedia;

        public event EventHandler<MusicPlayerEventArgs>? MusicPlayerEvent;
        public event EventHandler? AfterMusicPlayerEvent;

        public float Volume
        {
            get => _waveOutDevice?.Volume ?? 0;
            set
            {
                if(_waveOutDevice != null && value >= 0 && value <= 1)
                {
                    _waveOutDevice.Volume = value;
                }
            }
        }

        public long Position
        {
            get
            {
                var pos = (_audioFile as AudioFileReader)?.Position / (_audioFile as AudioFileReader)?.WaveFormat.AverageBytesPerSecond ?? 0;
                if (TotalTime < pos)
                    return TotalTime;

                return pos;
            }
            set
            {
                var audio = _audioFile as AudioFileReader;
                if (audio != null)
                {
                    audio.Position = (long?)(audio.WaveFormat.AverageBytesPerSecond * value) ?? 0;
                }
            }
        }

        public long TotalTime
        {
            get => (_audioFile as AudioFileReader)?.Length / (_audioFile as AudioFileReader)?.WaveFormat.AverageBytesPerSecond ?? 0;
        }

        public string PlayingSongPath => _currentMedia?.FilePath ?? "";

        public string PlayingSongName => Path.GetFileNameWithoutExtension(_currentMedia?.FilePath) ?? "";

        public PlaybackState PlayerState => _waveOutDevice?.PlaybackState ?? PlaybackState.Stopped;

        public MusicPlayerService(MediaStore mediaStore)
        {
            _mediaStore = mediaStore;
            _waveOutDevice = new WaveOut();
            _waveOutDevice.PlaybackStopped += OnStoppedPlay;
        }

        public void ChangeVolume(float volume)
        {
            throw new NotImplementedException();
        }

        public void Play(int mediaId)
        {
            OnPausePlay();
            _currentMedia = _mediaStore.Songs.FirstOrDefault(x => x.Id == mediaId);
            if (_currentMedia != null)
            {
                _waveOutDevice?.Stop();
                _waveOutDevice?.Dispose();

                try
                {
                    _audioFile = new AudioFileReader(_currentMedia.FilePath);
                    _waveOutDevice = new WaveOut();
                    _waveOutDevice.PlaybackStopped += OnStoppedPlay;
                    _waveOutDevice.Init(_audioFile);
                    _waveOutDevice.Play();
                    OnStartPlay();
                }
                catch
                {
                    Stop();
                }
            }
        }

        public void PlayPause()
        {
            if (_waveOutDevice == null) return;
            if (_waveOutDevice.PlaybackState == PlaybackState.Paused)
            {
                _waveOutDevice.Play();
                OnStartPlay();
            }
            else
            {
                _waveOutDevice.Pause();
                OnPausePlay();
            }
        }

        public void RePlay()
        {
            if (_audioFile != null)
            {
                _waveOutDevice?.Stop();
                _waveOutDevice?.Dispose();

                _waveOutDevice = new WaveOut();
                _waveOutDevice.PlaybackStopped += OnStoppedPlay;
                Position = 0;
                _waveOutDevice.Init(_audioFile);
                _waveOutDevice.Play();
                OnStartPlay();
            }
        }

        public void Stop()
        {
            _currentMedia = null;
            (_audioFile as AudioFileReader)?.Dispose();
            _audioFile = null;
            _waveOutDevice?.Stop();
            _waveOutDevice?.Dispose();

            OnStoppedPlay(this, null);

            _waveOutDevice = new WaveOut();
            _waveOutDevice.PlaybackStopped += OnStoppedPlay;
        }

        public void PlayNext(bool callStoppedPlay = true)
        {
            if (_currentMedia != null)
            {
                Skip:
                var tempmedia = _mediaStore.Songs.FirstOrDefault(x => x.Id > _currentMedia.Id && _currentMedia.PlayerlistId == x.PlayerlistId);
                if (tempmedia != null)
                {
                    _waveOutDevice?.Stop();
                    _waveOutDevice?.Dispose();

                    if(callStoppedPlay)
                        OnStoppedPlay(this, null);

                    _currentMedia = tempmedia;

                    try
                    {
                        _audioFile = new AudioFileReader(_currentMedia.FilePath);
                        _waveOutDevice = new WaveOut();
                        _waveOutDevice.PlaybackStopped += OnStoppedPlay;
                        _waveOutDevice.Init(_audioFile);
                        _waveOutDevice.Play();
                        OnStartPlay();
                    }
                    catch
                    {
                        if (_mediaStore.Songs.FirstOrDefault(x => x.Id > _currentMedia.Id && _currentMedia.PlayerlistId == x.PlayerlistId) == null)
                        {
                            Stop();
                        }
                        else
                        {
                            goto Skip;
                        }
                    }
                }
            }
        }

        public void PlayPrevious()
        {
            if (_currentMedia != null)
            {
                Prev:
                var tempmedia = _mediaStore.Songs.Reverse().FirstOrDefault(x => x.Id < _currentMedia.Id && _currentMedia.PlayerlistId == x.PlayerlistId);
                if (tempmedia != null)
                {
                    _waveOutDevice?.Stop();
                    _waveOutDevice?.Dispose();

                    OnStoppedPlay(this, null);

                    _currentMedia = tempmedia;

                    try
                    {
                        _audioFile = new AudioFileReader(_currentMedia.FilePath);
                        _waveOutDevice = new WaveOut();
                        _waveOutDevice.PlaybackStopped += OnStoppedPlay;
                        _waveOutDevice.Init(_audioFile);
                        _waveOutDevice.Play();
                        OnStartPlay();
                    }
                    catch
                    {
                        if (_mediaStore.Songs.FirstOrDefault(x => x.Id < _currentMedia.Id && _currentMedia.PlayerlistId == x.PlayerlistId) == null)
                        {
                            Stop();
                        }
                        else
                        {
                            goto Prev;
                        }
                    }
                }
            }
        }

        private void OnStoppedPlay(object? sender, StoppedEventArgs? e)
        {
            if (e == null)
            {
                MusicPlayerEvent?.Invoke(this, new MusicPlayerEventArgs(PlayerEventType.Stopped, _currentMedia, _audioFile));
            }
            else
            {
                MusicPlayerEvent?.Invoke(this, new MusicPlayerEventArgs(PlayerEventType.Finished, _currentMedia, _audioFile));
            }
            OnAfterPlay();
        }

        private void OnStartPlay()
        {
            MusicPlayerEvent?.Invoke(this, new MusicPlayerEventArgs(PlayerEventType.Playing, _currentMedia, _audioFile));
            OnAfterPlay();
        }

        private void OnPausePlay()
        {
            MusicPlayerEvent?.Invoke(this, new MusicPlayerEventArgs(PlayerEventType.Paused, _currentMedia, _audioFile));
            OnAfterPlay();
        }

        private void OnAfterPlay()
        {
            AfterMusicPlayerEvent?.Invoke(this, new EventArgs());
        }
    }
}
