using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Net;

namespace SteppyNetAPI.WebAPI.Models
{
    public class SecurityHelper
    {
        public static STEPPY_API_t_security_token CheckToken(string token)
        {
            STEPPY_APIContainer container = new STEPPY_APIContainer();
            var tokenData = container.STEPPY_API_t_security_token.Where<STEPPY_API_t_security_token>(x => x.security_token == token).ToList();
            if (tokenData.Count == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (tokenData.First().expired_date < DateTime.Now || tokenData.First().is_logout == true)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return tokenData.First();
        }
    }
}