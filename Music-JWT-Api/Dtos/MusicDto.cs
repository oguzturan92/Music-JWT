using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music_JWT_Api.Dtos
{
    public class MusicDto
    {
        public string PremiumName { get; set; }
        public List<Music> Musics { get; set; }

        public class Music
        {
            public string MusicName { get; set; }
        }
    }
}