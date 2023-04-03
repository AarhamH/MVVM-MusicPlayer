using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerData.DataEntities
{
    public class PlaylistEntity : BaseEntity
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        public DateTime? CreationDate { get; set; }
        public virtual ICollection<MediaEntity>? Songs { get; set; }
    }
}
