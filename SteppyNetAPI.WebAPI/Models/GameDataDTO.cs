using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class GameDataDTO
    {
        public int? IdUserShesop { get; set; }
        public int? IdLevel { get; set; }
        public DateTime? Date { get; set; }
        public Decimal? Mission1 { get; set; }
        public Decimal? Mission2 { get; set; }
        public Decimal? Mission3 { get; set; }
        public Decimal? Mission4 { get; set; }
        public Decimal? Point { get; set; }
        public int? Id { get; set; }
    }
}