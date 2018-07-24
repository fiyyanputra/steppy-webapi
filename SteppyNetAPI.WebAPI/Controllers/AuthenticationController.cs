using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models;
using System.Data.Entity;
using System.Web.Helpers;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class AuthenticationController : ApiController
    {

        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // POST api/authentication
        public HttpResponseMessage Post(AuthenticationData value)
        {     
            //check if username exist in database web sheshop
            var subquery = from u in container.UserProfiles
                           join p in container.webpages_Membership
                           on u.UserId equals p.UserId
                           where u.UserName == value.Email 
                           select u.UserId;

            //check if username exist in steppy user table
            var user = (from t1 in container.STEPPY_API_m_user
                        where subquery.Contains(t1.id_user_shesop)
                       select t1).ToList();

            //if user exist in web shesop but not yet exist in steppy user table
            if ((subquery.ToList().Count > 0) && (user.Count == 0))
            {
                //get data user from shesop db
                var checkUser = container.UserProfiles.Where<UserProfile>(x => x.UserName == value.Email).ToList();

                if (checkUser.Count > 0)
                {
                    STEPPY_API_m_user newUser = new STEPPY_API_m_user()
                    {
                        display_name = checkUser.First().UserName,
                        password =  HashingPassword(value.Password),
                        telp_number = checkUser.First().NoTelp,
                        join_date = DateTime.Now,
                        last_login = DateTime.Now,
                        id_user_shesop = checkUser.First().UserId
                    };

                    container.STEPPY_API_m_user.Add(newUser);
                    container.SaveChanges();

                    //create new token data
                    STEPPY_API_t_security_token _tokenData = container.STEPPY_API_t_security_token.Create();
                    _tokenData.id_user = newUser.id_user;
                    _tokenData.request_date = DateTime.Now;
                    _tokenData.expired_date = DateTime.Now.AddMonths(2);
                    _tokenData.is_logout = false;
                    _tokenData.security_token = Guid.NewGuid().ToString();
                    container.STEPPY_API_t_security_token.Add(_tokenData);
                    container.SaveChanges();

                    // create user profile steppy
                    STEPPY_API_t_user_profile newProfile = container.STEPPY_API_t_user_profile.Create();
                    newProfile.id_user = newUser.id_user;
                    newProfile.hi_score = 0;
                    newProfile.current_level = 1;
                    newProfile.current_experience = 0;
                    newProfile.next_level_experience = 100;
                    newProfile.gold = 0;
                    newProfile.diamond = 0;
                    newProfile.current_score = 0;
                    container.STEPPY_API_t_user_profile.Add(newProfile);
                    container.SaveChanges();

                    //recheck user count
                    user = (from t1 in container.STEPPY_API_m_user
                            where subquery.Contains(t1.id_user_shesop)
                            select t1).ToList();
                 }
            }
                        
            //if no user found, return null
            if (user.Count == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if(!Crypto.VerifyHashedPassword(user[0].password, value.Password))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            //updating last_login m_user info
            var userFound = user.First();
            userFound.last_login = DateTime.Now;

            var userd = (from u in container.UserProfiles
                        where u.UserId == userFound.id_user_shesop
                        select u).ToList().First();

            //set old token is_log out true
            STEPPY_API_t_security_token tokenData;
            var oldToken = container.STEPPY_API_t_security_token.Where<STEPPY_API_t_security_token>(x => x.is_logout == false 
                                                && x.expired_date > DateTime.Now 
                                                && x.id_user == userFound.id_user).ToList();

            if (oldToken.Count != 0)
            {
                tokenData = oldToken.First();
            }
            else
            {
                //generate new security token
                tokenData = new STEPPY_API_t_security_token();
                tokenData.id_user = userFound.id_user;
                tokenData.request_date = DateTime.Now;
                tokenData.expired_date = DateTime.Now.AddMonths(2);
                tokenData.is_logout = false;
                tokenData.security_token = Guid.NewGuid().ToString();
                container.STEPPY_API_t_security_token.Add(tokenData);
            }

            // save all changes
            container.SaveChanges();

            var profile = container.STEPPY_API_t_user_profile.Where(x => x.id_user == tokenData.id_user).First();

            // get all friends data
            var friends = container.STEPPY_API_v_user_friend.Where<STEPPY_API_v_user_friend>(friend => friend.id_user == tokenData.id_user).ToList();
            var frienddto = (from fr in friends
                            join pf in container.STEPPY_API_t_user_profile
                                on fr.id_user equals pf.id_user
                            select new FriendDataDTO
                            {
                                IdUser = fr.id_user,
                                FriendIdUser = fr.friend_id_user,
                                DisplayName = fr.display_name,
                                HiScore = pf.hi_score,
                                Level = pf.current_level,
                                TelpNumber = fr.telp_number,
                                FriendDetailUrl = Url.Link("TransactApi", new { controller = "friend", token = tokenData.security_token, id = fr.id_contact }),
                                IdContact = fr.id_contact                                
                            }).ToList();

            //get game record
            var usergamedata = container.STEPPY_Gamification_Record.Where<STEPPY_Gamification_Record>(g => g.IdUserShesop == userFound.id_user_shesop).ToList();
            List<GameDataDTO> usergamedto = new List<GameDataDTO>();
            if (usergamedata.Count > 0)
            {
                usergamedto = (from d in usergamedata
                                   select new GameDataDTO
                                   {
                                       Id = d.Id,
                                       IdUserShesop = d.IdUserShesop,
                                       Date = d.Date,
                                       Mission1 = d.Mission1,
                                       Mission2 = d.Mission2,
                                       Mission3 = d.Mission3,
                                       Mission4 = d.Mission4,
                                       Point = d.Point,
                                       IdLevel = d.IdLevel
                                   }).ToList();
            }
            else
            {
                var gdto = new GameDataDTO()
                {
                    Mission1 = 0,
                    Mission2 = 0,
                    Mission3 = 0,
                    Mission4 = 0,
                    Point = 0,
                    IdLevel = 0
                };
                usergamedto.Add(gdto);
            }

            //convert user data info into data tranferable object for easy client consuming 
            var userdto = user.Select(x => new UserDataDTO()
                                                    {
                                                        IdUser = x.id_user,
                                                        IdUserShesop = x.id_user_shesop,
                                                        DisplayName = x.display_name,
                                                        Email = userd.UserName,
                                                        Gender = userd.JenisKelamin,
                                                        Age = userd.Umur,
                                                        Height = userd.Tinggi,
                                                        Weight = userd.BeratBadan,
                                                        TelpNumber = x.telp_number,
                                                        JoinDate = x.join_date,
                                                        /*UserContacts = x.STEPPY_API_m_contact
                                                                .Select(c => new UserContactDTO()
                                                                        {
                                                                            IdContact = c.id_contact,
                                                                            TelpNumber = c.telp_number
                                                                        }).ToList(),*/
                                                        FriendProfiles = frienddto,
                                                        Token = tokenData.security_token,
                                                        ExpiredDate = tokenData.expired_date,
                                                        Profile = new ProfileDTO()
                                                        {
                                                            IdUser = profile.id_user,
                                                            IdProfile = profile.id_profile,
                                                            DisplayName = x.display_name,
                                                            HiScore = profile.hi_score,
                                                            CurrentLevel = profile.current_level,
                                                            CurrentExperience = profile.current_experience,
                                                            NextLevelExperience = profile.next_level_experience,
                                                            CurrentScore = profile.current_score,
                                                            Gold = profile.gold,
                                                            Diamond = profile.diamond
                                                        },
                                                        FriendsUrl = Url.Link("TransactApi", new { controller = "friend", token = tokenData.security_token }),
                                                        ProfileUrl = Url.Link("TransactApi", new { controller = "profile", token = tokenData.security_token }),
                                                        UserGameData = usergamedto.Last()
                                                    }
                                                 ).ToList();
           
            return Request.CreateResponse(HttpStatusCode.OK, userdto.First());
        }

        public string HashingPassword(string password)
        {
            var hashedPassword = Crypto.HashPassword(password);
            bool tr = Crypto.VerifyHashedPassword(hashedPassword, password);

            return hashedPassword;
        }
    }
}