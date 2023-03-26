using dotnet_player_client.Services;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class TogglePlayerCommand : BaseCommand
    {
        private readonly IPlayerService _musicService;
        
        public TogglePlayerCommand(IPlayerService musicService)
        {
            _musicService = musicService;
        }

        public override void Execute(object? parameter)
        {
            if(_musicService.PlayerState != PlaybackState.Stopped)
            {
                _musicService.PlayPause();
            }
            else
            {
                _musicService.RePlay();
            }
        }
    }
}
