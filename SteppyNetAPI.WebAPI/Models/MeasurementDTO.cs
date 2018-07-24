using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class MeasurementDTO
    {
        public int IdMeasurement { get; set; }
        public decimal Value { get; set; }
        public long TimeStamp { get; set; }
        public string MeasurementType { get; set; }
    }
}