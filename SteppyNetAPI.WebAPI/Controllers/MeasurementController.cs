using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class MeasurementController : ApiController
    {

        STEPPY_APIContainer container = new STEPPY_APIContainer();
        

        // POST api/measurement
        public HttpResponseMessage Post(string token, List<MeasurementDTO> values)
        {
            // checking user token
            var tokenData = SecurityHelper.CheckToken(token);

            foreach (var value in values)
            {
                // creating new measurement data
                STEPPY_API_t_measurement measurement = container.STEPPY_API_t_measurement.Create();
                measurement.id_measurement = value.IdMeasurement;
                measurement.id_measurement_type = int.Parse(value.MeasurementType);
                measurement.value = value.Value;
                measurement.timestamp = value.TimeStamp;
                measurement.id_user = tokenData.id_user;
                measurement.insert_time = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                container.STEPPY_API_t_measurement.Add(measurement);
            }

            // save all changes
            container.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

    }
}