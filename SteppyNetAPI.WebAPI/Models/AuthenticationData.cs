using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class AuthenticationData
    {
        public string DisplayName { get; set; }
        public string TelpNumber { get; set; }
        public string CountryCode { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }

    }
}