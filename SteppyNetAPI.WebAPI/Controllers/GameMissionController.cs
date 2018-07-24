using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models;
using System.Diagnostics;
using System.Globalization;
using System.Web.Script.Serialization;
using SteppyNetAPI.WebAPI.Class;


namespace SteppyNetAPI.WebAPI.Controllers
{
    public class GameMissionController : ApiController
    {
        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // Get api/gamemission
        public HttpResponseMessage Get()
        {
            var dtMission = container.STEPPY_Gamification_Mission.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, dtMission);
        }
    }
}
