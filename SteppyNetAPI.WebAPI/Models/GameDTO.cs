using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class GameDTO
    {
        public String IdLevel { get; set; }
        public String IdUserShesop { get; set; }
        public String Mission1 { get; set; }
        public String Mission2 { get; set; }
        public String Mission3 { get; set; }
        public String Mission4 { get; set; }
        public String Date { get; set; }
        public String Point { get; set;}
    }
}