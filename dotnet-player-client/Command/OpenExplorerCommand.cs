using dotnet_player_client.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerClient.Commands
{
    public class OpenExplorerCommand : BaseCommand
    {
        public OpenExplorerCommand()
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
