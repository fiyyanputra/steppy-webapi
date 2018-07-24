using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models;
using SteppyNetAPI.WebAPI.Class;
using System.Globalization;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class GameRecordController : ApiController
    {
        STEPPY_APIContainer container = new STEPPY_APIContainer();
        
        // POST api/gamerecord
        public HttpResponseMessage Post(GameDTO value)
        {
            DateTime pDate = DateTime.ParseExact(value.Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int idShesop =  Int32.Parse(value.IdUserShesop);

            STEPPY_Gamification_Record newGameRecord = new STEPPY_Gamification_Record()
            {
                IdUserShesop = idShesop,
                Date = pDate,
                Mission1 = Decimal.Parse(value.Mission1),
                Mission2 = Decimal.Parse(value.Mission2),
                Mission3 = Decimal.Parse(value.Mission3),
                Mission4 = Decimal.Parse(value.Mission4),
                IdLevel = Int32.Parse(value.IdLevel),
                Point = Decimal.Parse(value.Point)
            };

            container.STEPPY_Gamification_Record.Add(newGameRecord);
            container.SaveChanges();

            //get game mission
            var mission = container.STEPPY_Gamification_Mission.ToList();
            var missiondto = (from m in mission
                              select new MissionDTO
                              {
                                  IdLevel = m.IdLevel,
                                  Mission1 = m.Mission1,
                                  Mission2 = m.Mission2,
                                  Mission3 = m.Mission3,
                                  Mission4 = m.Mission4
                              }).ToList();
        
            var userGame = new UserGameDTO()
            {
                dataLastGame = newGameRecord,
                mission = missiondto
            };

            //return Request.CreateResponse(HttpStatusCode.Created, container.STEPPY_Gamification_Record.Where<STEPPY_Gamification_Record>(s => s.IdUserShesop == idShesop).ToList().Last());
            return Request.CreateResponse(HttpStatusCode.Created, userGame);
        }
    }
}
