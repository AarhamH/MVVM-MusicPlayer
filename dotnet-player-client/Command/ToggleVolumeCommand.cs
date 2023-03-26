using dotnet_player_client.Models;
using dotnet_player_client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class ToggleVolumeCommand : BaseCommand
    {
        private readonly PlayerVM _player;
        private int _volume;

        public ToggleVolumeCommand(PlayerVM player)
        {
            _player = player;
        }

        public override void Execute(object? parameter)
        {
            if(parameter is int volume)
            {
                if(volume == 0)
                {
                    _player.Volume = _volume;
                }
                else
                {
                    _volume = _player.Volume;
                    _player.Volume = 0;
                }
            }
        }
    }
}
