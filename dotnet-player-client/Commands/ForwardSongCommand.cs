using dotnet_player_client.Services;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Commands
{
    public class ForwardSongCommand : CommandBase
    {
        private readonly IMusicPlayerService _musicService;
        public ForwardSongCommand(IMusicPlayerService musicService)
        {
            _musicService = musicService;
        }

        public override void Execute(object? parameter)
        {
            _musicService.PlayNext();
        }
    }
}
