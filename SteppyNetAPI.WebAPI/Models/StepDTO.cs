using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class StepDTO
    {
        public string UserId { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Step { get; set; }
        public string Calori { get; set; }
        public string Keterangan { get; set; }
        public string IdUserShesop { get; set; }
        public string Sensor { get; set; }
        public string Distance { get; set; }
    }
}