using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Arguments
{
    public class PLBrowseArgs : EventArgs
    {
        public int PlayListID { get; set; }

        public PLBrowseArgs(int playListID)
        {
            PlayListID = playListID;
        }
    }
}
