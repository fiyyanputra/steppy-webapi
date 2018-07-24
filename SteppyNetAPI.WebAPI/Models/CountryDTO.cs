using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class CountryDTO
    {
        public int IdCountry { get; set; }
        public string CountryName { get; set; }
        public string CountryPhonePrefix { get; set; }
    }
}