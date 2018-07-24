using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models.OpenMeasurement
{
    public class OpenDataMeasurementViewModel
    {
        public long? IdMeasurement { get; set; }
        public string UserId { get; set; }
        public string DeviceType { get; set; }
        public string Type { get; set; }
        public decimal? Value { get; set; }
        public long? TimeStamp { get; set; }
        public int? IdMeasurementType { get; set; }
        public string MeasurementUnit { get; set; }
        public long? InsertedTime { get; set; }
    }
}