using dotnet_player_client.Services;
using dotnet_player_client.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class GoForwardCommand : BaseCommand
    {
        private readonly IPlayerService _musicService;
        public GoForwardCommand(IPlayerService musicService)
        {
            _musicService = musicService;
        }

        public override void Execute(object? parameter) 
        {
            _musicService.PlayNext();
        }
    }
}
