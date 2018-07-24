using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class StepBandDTO
    {
        public string UserId { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Step { get; set; }
        public string Keterangan { get; set; }
        public string IdUserShesop { get; set; }
        public string Calorie { get; set; }
        public string Distance { get; set; }
       
    }
}