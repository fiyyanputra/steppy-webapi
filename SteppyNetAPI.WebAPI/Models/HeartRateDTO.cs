using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class HeartRateDTO
    {
        public string UserId { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string HeartRate { get; set; }
        public string Keterangan { get; set; }
        public string IdUserShesop { get; set; }
        public string Pace { get; set; }
        public string Speed { get; set; }
        public string Temperature { get; set; }
        public string UVlevel { get; set; }
    }
}