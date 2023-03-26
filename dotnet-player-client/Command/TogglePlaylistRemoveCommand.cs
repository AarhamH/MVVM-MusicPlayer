using dotnet_player_client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class TogglePlaylistRemoveCommand : BaseCommand
    {
        private readonly ToolbarVM _toolBarVM;
        public TogglePlaylistRemoveCommand(ToolbarVM toolBarVM)
        {
            _toolBarVM = toolBarVM;
        }

        public override void Execute(object? parameter)
        {
            _toolBarVM.IsRemoveActive = !_toolBarVM.IsRemoveActive;
        }

    }
}
