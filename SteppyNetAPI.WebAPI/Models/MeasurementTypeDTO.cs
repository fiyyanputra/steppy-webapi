using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class MeasurementTypeDTO
    {
        public int IdMeasurementType { get; set; }
        public string MeasurementTypeName { get; set; }
        public string MeasurementTypeUnit { get; set; }
        public string MeasurementDescription { get; set; }
    }
}