using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class ProfileDTO
    {
        public int? IdProfile { get; set; }
        public string DisplayName { get; set; }
        public int? CurrentLevel { get; set; }
        public int? IdUser { get; set; }
        public decimal? HiScore { get; set; }
        public decimal? CurrentExperience { get; set; }
        public decimal? NextLevelExperience { get; set; }
        public decimal? Gold { get; set; }
        public int? Diamond { get; set; }
        public decimal? CurrentScore { get; set; }
        public string ProfileImage { get; set; }
    }
}