using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerData.DataEntities
{
    public class MediaEntity : BaseEntity
    {
        [MaxLength(256)]
        public string? FilePath { get; set; }
        public virtual int? PlayerlistId { get; set; }
        public virtual PlaylistEntity? Playerlist { get; set; }
    }
}
