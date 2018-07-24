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
    public class HeartRateController : ApiController
    {
        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // POST api/heartrate
        public HttpResponseMessage Post(HeartRateDTO value)
        {
            DateTime pDate = DateTime.ParseExact(value.Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            STEPPY_API_BAND_Heartrate hr = new STEPPY_API_BAND_Heartrate()
            {
                UserID = value.UserId,
                tanggal = DateTime.ParseExact(value.Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                jam_mulai = DateTime.ParseExact(value.StartTime, "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay,
                jam_akhir = DateTime.ParseExact(value.EndTime, "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay,
                heartrate = int.Parse(value.HeartRate),
                user_id_shesop = value.IdUserShesop,
                pace = Decimal.Parse(value.Pace),
                speed = Decimal.Parse(value.Speed),
                temperature = Decimal.Parse(value.Temperature),
                uv_level = value.UVlevel
            };
            
            container.STEPPY_API_BAND_Heartrate.Add(hr);
            container.SaveChanges();

            var response = Request.CreateResponse(HttpStatusCode.Created, container.STEPPY_API_BAND_Heartrate.Where(s => s.user_id_shesop == value.IdUserShesop).Where(s => s.tanggal == pDate).ToList().Last());

            return response;
        }

        // GET api/heartrate/
        public HttpResponseMessage Get(string idShesop, string date)
        {
            DateTime pDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var hr = container.STEPPY_API_BAND_Heartrate.Where<STEPPY_API_BAND_Heartrate>(s => s.user_id_shesop == idShesop).Where(s=>s.tanggal == pDate) .Select(s=>s.heartrate).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, hr);
        }
    }
}
