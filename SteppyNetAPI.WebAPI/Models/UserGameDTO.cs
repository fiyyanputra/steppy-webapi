using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class UserGameDTO
    {
        public STEPPY_Gamification_Record dataLastGame { get; set; }
        public List<MissionDTO> mission { get; set; }
    }
}