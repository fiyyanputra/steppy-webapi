using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models.OpenMeasurement;
using SteppyNetAPI.WebAPI.Models;
using SteppyNetAPI.WebAPI.Class;

namespace SteppyNetAPI.WebAPI.Controllers.OpenMeasurement
{
    public class OpenMeasurementController : ApiController
    {
        OpenMeasurementConnection container = new OpenMeasurementConnection();
        STEPPY_APIContainer steppyContainer = new STEPPY_APIContainer();
        // GET api/openmeasurement
        public IEnumerable<OpenDataMeasurement> Get()
        {

            //DateTime startweek = 
            //DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            return 
            new OpenDataMeasurement[] 
            { 
                new OpenDataMeasurement(){
                    UserId = DateTimeHelper.GetFirstWeekEpoch(DateTime.Now).ToString(),
                    DeviceType = "MIRROR",
                    Type = "HR",
                    Value = 89,
                    TimeStamp = (long)(DateTime.Now - new DateTime(1970,1,1)).TotalMilliseconds
                }, 
                new OpenDataMeasurement(){
                    UserId = "9091829",
                    DeviceType = "MIRROR",
                    Type = "RR",
                    Value = 20,
                    TimeStamp = (long)(DateTime.Now - new DateTime(1970,1,1)).TotalMilliseconds
                }
            };
        }

        // GET api/openmeasurement/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/openmeasurement
        public HttpResponseMessage Post(List<OpenDataMeasurement> data)
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long ms = (long) t.TotalMilliseconds;

            foreach(var item in data){
                STEPPY_API_t_loose_measurement newMeasurement = container.STEPPY_API_t_loose_measurement.Create();
                newMeasurement.inserted_timestamp = ms;
                newMeasurement.device_type = item.DeviceType;
                newMeasurement.epoch_timestamp = item.TimeStamp;
                newMeasurement.measurement_type = (from type in steppyContainer.STEPPY_API_m_measurement_type
                                                  where type.measurement_type_name == item.Type
                                                  select type.id_measurement_type).First();
                newMeasurement.id_user = item.UserId;
                newMeasurement.value = item.Value;

                container.STEPPY_API_t_loose_measurement.Add(newMeasurement);
            }

            container.SaveChanges();
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/openmeasurement/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/openmeasurement/5
        public void Delete(int id)
        {
        }
    }
}
