using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class ProfileController : ApiController
    {
        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // GET api/profile/[token]
        public HttpResponseMessage Get(string token)
        {
            // check user token
            var tokenData = SecurityHelper.CheckToken(token);



            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        // GET api/profile/[token]/5
        public string Get(string token, int id)
        {
            return "value";
        }

        // POST api/profile
        //method for update profile shesop user
        public HttpResponseMessage Post(string token, int id, AuthenticationData value)
        {
            var dataProfile = container.UserProfiles.Where<UserProfile>(x => x.UserId == id).ToList();
            if (value != null)
            {
                if (dataProfile.Count > 0)
                {
                    if (value.DisplayName != null)
                    {
                        //update on tabel steppy_m_user
                        var disname = container.STEPPY_API_m_user.Where<STEPPY_API_m_user>(x => x.id_user_shesop == id).ToList();
                        if (disname.Count > 0)
                        {
                            disname.First().display_name = value.DisplayName;
                        }
                    }

                    if (value.Age > 0)
                    {
                        dataProfile.First().Umur = value.Age;
                    }
                    if (value.Height > 0)
                    {
                        dataProfile.First().Tinggi = value.Height;
                    }
                    if (value.Weight > 0)
                    {
                        dataProfile.First().BeratBadan = value.Weight;
                    }
                    if (value.Gender != null)
                    {
                        dataProfile.First().JenisKelamin = value.Gender;
                    }
                    if (value.TelpNumber != null)
                    {
                        dataProfile.First().NoTelp = value.TelpNumber;
                    }
                }
            }
            container.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, dataProfile.First());
        }

        // PUT api/profile/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/profile/5
        public void Delete(int id)
        {
        }
    }
}
