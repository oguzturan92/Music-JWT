using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music_JWT_Api.Dtos;
using static Music_JWT_Api.Dtos.MusicDto;

namespace Music_JWT_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            List<MusicList> musicsList = new()
            {
                new(){MusicName="Bohemian Rhapsody - Queen", PremiumName="EliteCraft"},
                new(){MusicName="Shape of You - Ed Sheeran", PremiumName="EliteCraft"},
                new(){MusicName="Billie Jean - Michael Jackson", PremiumName="EliteCraft"},
                new(){MusicName="Rolling in the Deep - Adele", PremiumName="EliteCraft"},
                new(){MusicName="Hotel California - Eagles", PremiumName="EliteCraft"},
                new(){MusicName="Someone Like You - Adele", PremiumName="PlatinumPlus"},
                new(){MusicName="Smells Like Teen Spirit - Nirvana", PremiumName="PlatinumPlus"},
                new(){MusicName="Blinding Lights - The Weeknd", PremiumName="PlatinumPlus"},
                new(){MusicName="Wonderwall - Oasis", PremiumName="PlatinumPlus"},
                new(){MusicName="Let Her Go - Passenger", PremiumName="PlatinumPlus"},
                new(){MusicName="Imagine - John Lennon", PremiumName="RoyalEdge"},
                new(){MusicName="Hello - Adele", PremiumName="RoyalEdge"},
                new(){MusicName="Stayin' Alive - Bee Gees", PremiumName="RoyalEdge"},
                new(){MusicName="Hey Jude - The Beatles", PremiumName="RoyalEdge"},
                new(){MusicName="Havana - Camila Cabello", PremiumName="RoyalEdge"},
                new(){MusicName="Uptown Funk - Mark Ronson ft. Bruno Mars", PremiumName="SupremeLine"},
                new(){MusicName="Bad Guy - Billie Eilish", PremiumName="SupremeLine"},
                new(){MusicName="Lose Yourself - Eminem", PremiumName="SupremeLine"},
                new(){MusicName="Stairway to Heaven - Led Zeppelin", PremiumName="SupremeLine"},
                new(){MusicName="Perfect - Ed Sheeran", PremiumName="SupremeLine"},
            };

            MusicDto musicDto = new();    
            if (User.IsInRole("EliteCraft"))
            {
                musicDto = new MusicDto{
                    PremiumName = "EliteCraft",
                    Musics = musicsList.Where(i => i.PremiumName == "EliteCraft").Select(i => new Music{
                        MusicName = i.MusicName
                    }).ToList()
                };
            }
            if (User.IsInRole("PlatinumPlus"))
            {
                musicDto = new MusicDto{
                    PremiumName = "PlatinumPlus",
                    Musics = musicsList.Where(i => i.PremiumName == "PlatinumPlus").Select(i => new Music{
                        MusicName = i.MusicName
                    }).ToList()
                };
            }
            if (User.IsInRole("RoyalEdge"))
            {
                musicDto = new MusicDto{
                    PremiumName = "RoyalEdge",
                    Musics = musicsList.Where(i => i.PremiumName == "RoyalEdge").Select(i => new Music{
                        MusicName = i.MusicName
                    }).ToList()
                };
            }
            if (User.IsInRole("SupremeLine"))
            {
                musicDto = new MusicDto{
                    PremiumName = "SupremeLine",
                    Musics = musicsList.Where(i => i.PremiumName == "SupremeLine").Select(i => new Music{
                        MusicName = i.MusicName
                    }).ToList()
                };
            }
            return Ok(musicDto);
        }
    }
}