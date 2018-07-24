using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class CountryController : ApiController
    {

        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // GET api/country
        public HttpResponseMessage Get()
        {
            // collect all country data
            var countryData = container.STEPPY_API_m_country.OrderBy(x => x.country_name).Select(x => 
                                                        new CountryDTO() {IdCountry = x.id_country, 
                                                                          CountryName = x.country_name, 
                                                                          CountryPhonePrefix = x.country_phone_prefix }
                                                                    );

            return Request.CreateResponse(HttpStatusCode.OK, countryData);
        }

        // GET api/country/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/country
        public void Post([FromBody]string value)
        {
        }

        // PUT api/country/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/country/5
        public void Delete(int id)
        {
        }
    }
}
