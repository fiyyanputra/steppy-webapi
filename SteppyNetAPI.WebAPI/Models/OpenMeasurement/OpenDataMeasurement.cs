using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models.OpenMeasurement
{
    public class OpenDataMeasurement
    {
        public string UserId { get; set; }
        public string DeviceType { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public long TimeStamp { get; set; }
    }
}