using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Class;
using SteppyNetAPI.WebAPI.Models;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class FriendController : ApiController
    {
        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // GET api/friend/token/
        public HttpResponseMessage Get(string token)
        {
            // checking user token
            var tokenData = SecurityHelper.CheckToken(token);

            DateTime today = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, 0));
            long todayEpoch = (long)DateTimeHelper.DateToEpoch(today);
            long todayDateEpoch = (long)DateTimeHelper.DateToEpoch(today.Date);
            long firstWeekEpoch = (long)DateTimeHelper.GetFirstWeekEpoch(today);
                        
            // get friend profile
            var frienddto = (from f in container.STEPPY_API_v_user_friend
                             join p in container.STEPPY_API_t_user_profile
                             on f.friend_id_user equals p.id_user
                             where f.id_user == tokenData.id_user
                             select new FriendDataDTO()
                           {
                               FriendIdUser = f.friend_id_user,
                               DisplayName = f.display_name,
                               TelpNumber = f.telp_number,
                               IdContact = f.id_contact,
                               HiScore = p.hi_score,
                               Level = p.current_level,
                               IdUser = tokenData.id_user,
                           }).ToList();

            // fill the unassigned fields
            foreach (var f in frienddto)
            {
                f.TodayStep = (int?)container.STEPPY_API_t_measurement.Where(y => y.id_user == f.FriendIdUser)
                                                .Where(y => y.timestamp > todayDateEpoch
                                                && y.timestamp <= todayEpoch
                                                && y.STEPPY_API_m_measurement_type.measurement_type_name.Contains("STEP")).Sum(y => y.value);
                f.WeeklyStep = (int?)container.STEPPY_API_t_measurement.Where(y => y.id_user == f.FriendIdUser)
                                                .Where(y => y.timestamp > firstWeekEpoch
                                                && y.timestamp <= todayEpoch
                                                && y.STEPPY_API_m_measurement_type.measurement_type_name.Contains("STEP")).Sum(y => y.value);
                f.FriendDetailUrl = Url.Link("TransactApi", new { controller = "friend", token = tokenData.security_token, id = f.IdContact });
            }
                        
            var response = Request.CreateResponse(HttpStatusCode.OK, frienddto.OrderByDescending(x => x.WeeklyStep));
            return response;
        }

        // GET api/friend/token/id
        public HttpResponseMessage Get(string token, int id)
        {
            // checking user token
            var tokenData = SecurityHelper.CheckToken(token);

            DateTime today = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, 0));
            long todayEpoch = (long) DateTimeHelper.DateToEpoch(today);
            long todayDateEpoch = (long) DateTimeHelper.DateToEpoch(today.Date);
            long firstWeekEpoch = (long) DateTimeHelper.GetFirstWeekEpoch(today);

            // get friend profile
            var frienddto = (from f in container.STEPPY_API_v_user_friend
                             join p in container.STEPPY_API_t_user_profile
                             on f.friend_id_user equals p.id_user
                             where f.id_user == tokenData.id_user && f.id_contact == id
                             select new FriendDataDTO()
                             {
                                 FriendIdUser = f.friend_id_user,
                                 DisplayName = f.display_name,
                                 TelpNumber = f.telp_number,
                                 IdContact = f.id_contact,
                                 HiScore = p.hi_score,
                                 Level = p.current_level,
                                 IdUser = tokenData.id_user,
                             }).ToList();

            // fill the unassigned fields
            foreach (var f in frienddto)
            {
                f.TodayStep = (int?)container.STEPPY_API_t_measurement.Where(y => y.id_user == f.FriendIdUser)
                                                .Where(y => y.timestamp > todayDateEpoch
                                                && y.timestamp <= todayEpoch
                                                && y.STEPPY_API_m_measurement_type.measurement_type_name.Contains("STEP")).Sum(y => y.value);
                f.WeeklyStep = (int?)container.STEPPY_API_t_measurement.Where(y => y.id_user == f.FriendIdUser)
                                                .Where(y => y.timestamp > firstWeekEpoch
                                                && y.timestamp <= todayEpoch
                                                && y.STEPPY_API_m_measurement_type.measurement_type_name.Contains("STEP")).Sum(y => y.value);
                f.FriendDetailUrl = Url.Link("TransactApi", new { controller = "friend", token = tokenData.security_token, id = f.IdContact });
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, frienddto);
            return response;
        }

        
        // POST api/values
        public HttpResponseMessage Post(string token, ContactTelpNumbers value)
        {
            // checking user token
            var tokenData = SecurityHelper.CheckToken(token);

            // splitting long contacts string
            string[] telpNumbers = value.TelpNumbers.Split(',');

            foreach (var telpNumber in telpNumbers)
            {
                // checking whether the contact exist or not
                if ((container.STEPPY_API_m_contact
                    .Where(x => x.id_user == tokenData.id_user && x.telp_number == telpNumber)
                    .Count() == 0) 
                    && (telpNumber.Length > 9))
                {
                    // create new contact data
                    var contact = container.STEPPY_API_m_contact.Create();
                    contact.id_user = tokenData.id_user;
                    contact.telp_number = telpNumber;

                    // checking if contact phone number exist in m_user table or not
                    var user = container.STEPPY_API_m_user
                        .Where(x => ("0" + x.telp_number) == telpNumber 
                            || (x.STEPPY_API_m_country.country_phone_prefix + x.telp_number == telpNumber))
                            .ToList();
                    
                    // assigning contact status is registered or not
                    if (user.Count > 0)
                        contact.is_registered = true;
                    else contact.is_registered = false;

                    container.STEPPY_API_m_contact.Add(contact);
                    container.SaveChanges();
                }
            }            

            return Get(token);
        }
    }
}
