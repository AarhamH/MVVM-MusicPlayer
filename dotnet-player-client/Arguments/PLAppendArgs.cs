using dotnet_player_data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_player_client.Arguments
{
    public class PLAppendArgs
    {
        public IEnumerable<SongObjects> Songs { get; set; }
        public PLAppendArgs(IEnumerable<SongObjects> songs) {
            Songs = songs;
        }
    }
}
