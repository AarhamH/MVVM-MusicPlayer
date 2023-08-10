using PalmClient.Services;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Commands
{
    public class PlaySpecificSongCommand : CommandBase
    {
        private readonly IMusicPlayerService _musicService;
        public PlaySpecificSongCommand(IMusicPlayerService musicService)
        {
            _musicService = musicService;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is int SongId)
            {
                if (_musicService.CurrentMedia?.Id == SongId)
                {
                    if (_musicService.PlayerState != PlaybackState.Stopped)
                    {
                        _musicService.PlayPause();
                    }
                    else _musicService.RePlay();
                }
                else
                {
                    _musicService.Play(SongId);
                }
            }
        }
    }
}
