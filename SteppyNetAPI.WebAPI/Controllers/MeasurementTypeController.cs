using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class MeasurementTypeController : ApiController
    {
        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // GET api/measurementtype/token/
        public HttpResponseMessage Get(string token)
        {
            // check user's token
            var tokenData = SecurityHelper.CheckToken(token);

            // get measurement type
            var result = container.STEPPY_API_m_measurement_type.Select(x => 
                                    new MeasurementTypeDTO() { 
                                                                IdMeasurementType = x.id_measurement_type,
                                                                MeasurementTypeName = x.measurement_type_name,
                                                                MeasurementTypeUnit = x.measurement_type_unit,
                                                                MeasurementDescription = x.measurement_description
                                                                }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        
        // PUT api/measurementtype/token/5
        public HttpResponseMessage Post(string token, List<MeasurementTypeDTO> values)
        {
            // check user token
            var tokenData = SecurityHelper.CheckToken(token);

            foreach (var value in values)
            {
                // create new measurement type
                var measurementType = container.STEPPY_API_m_measurement_type.Create();
                measurementType.measurement_type_name = value.MeasurementTypeName;
                measurementType.measurement_type_unit = value.MeasurementTypeUnit;
                measurementType.measurement_description = value.MeasurementDescription;
                container.STEPPY_API_m_measurement_type.Add(measurementType);
            }

            container.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // DELETE api/measurementtype/5
        public void Delete(int id)
        {

        }
    }
}
