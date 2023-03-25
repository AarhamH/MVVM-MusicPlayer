using dotnet_player_client.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Command
{
    public class OpenExplorerAtPathCommand : BaseCommand
    {
        public OpenExplorerAtPathCommand()
        {

        }

        public override void Execute(object? parameter)
        {
            if(parameter is string path)
            {
                string argument = "/select, \"" + path + "\"";

                Process.Start("explorer.exe", argument);
            }
        }
    }
}
