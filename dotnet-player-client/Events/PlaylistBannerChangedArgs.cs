using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Events
{
    public class PlaylistBannerChangedArgs
    {
        public int Id { get; set; }
        public string Banner { get; set; }

        public PlaylistBannerChangedArgs(int id, string url)
        {
            Id = id;
            Banner = url;
        }
    }

}
