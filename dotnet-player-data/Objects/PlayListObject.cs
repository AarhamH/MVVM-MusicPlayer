using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dotnet_player_data.Objects
{
    public class PlayListObject : IDObject
    {
        [MaxLength(40)]
        public string? PLTitle { get; set; }
        public virtual ICollection<SongObjects>? SongCollection { get; set; }
    }
}
