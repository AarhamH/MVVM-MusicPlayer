using PalmClient.Core;
using PalmClient.Services;
using PalmClient.ViewModels;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Commands
{
    public class TogglePlaylistRemoveCommand : CommandBase
    {
        private readonly ToolbarViewModel _toolbar;
        public TogglePlaylistRemoveCommand(ToolbarViewModel toolbar)
        {
            _toolbar = toolbar;
        }

        public override void Execute(object? parameter)
        {

        }
    }
}
