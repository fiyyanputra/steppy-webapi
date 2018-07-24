using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using SteppyNetAPI.WebAPI.Models;
using System.Web.Helpers;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class RegistrationController : ApiController
    {
        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // POST api/registration
        public HttpResponseMessage Post(AuthenticationData value)
        {
            // checking whether new user already exist on database or not
            var subquery = from u in container.UserProfiles
                           join p in container.webpages_Membership
                           on u.UserId equals p.UserId
                           where u.UserName == value.Email 
                           select u.UserId;

            var checkUser = (from t1 in container.STEPPY_API_m_user
                        where subquery.Contains(t1.id_user_shesop)
                        select t1).ToList();

            if (checkUser.Count != 0)
                return Request.CreateResponse<string>(HttpStatusCode.Forbidden, "User already exist!");

            //create new user and then save to database shesop
            UserProfile newUserProfile = new UserProfile()
            {
                UserName = value.Email,
                NoTelp = value.TelpNumber,
                Umur = value.Age,
                BeratBadan = value.Weight,
                Tinggi = value.Height,
                JenisKelamin = value.Gender
            };

            container.UserProfiles.Add(newUserProfile);
            container.SaveChanges();

            webpages_Membership newUserWebpages = new webpages_Membership()
            {
                UserId = newUserProfile.UserId,
                IsConfirmed = true,
                Password = HashingPassword(value.Password),
                PasswordFailuresSinceLastSuccess = 0,
                PasswordSalt = "",
                PasswordVerificationToken = "",
                CreateDate = DateTime.Now,
                PasswordChangedDate = DateTime.Now,

            };

            container.webpages_Membership.Add(newUserWebpages);
            container.SaveChanges();

            webpages_UsersInRoles newUserRole = new webpages_UsersInRoles()
            {
                UserId = newUserProfile.UserId,
                RoleId = 3,
            };

            container.webpages_UsersInRoles.Add(newUserRole);
            container.SaveChanges();

            DCHANNEL_User_Patient profile = new DCHANNEL_User_Patient()
            {
                Patient_Name = newUserProfile.UserName,
                User_Id = newUserProfile.UserId,
                Email = newUserProfile.UserName,
                Join_date = DateTime.Now,
                generated_guid = Guid.NewGuid().ToString()
            };

            container.DCHANNEL_User_Patient.Add(profile);
            container.SaveChanges();

            //end of create new user and then save to database shesop

            // register to table user steppy
            STEPPY_API_m_user newUser = new STEPPY_API_m_user() { 
                                            display_name = value.DisplayName,
                                            password = newUserWebpages.Password,
                                            telp_number = value.TelpNumber,
                                            join_date = DateTime.Now,
                                            last_login = DateTime.Now,
                                            id_user_shesop = newUserWebpages.UserId
                                            //id_country = countryData.id_country
                                           };


            container.STEPPY_API_m_user.Add(newUser);
            container.SaveChanges();

            //create new token data
            STEPPY_API_t_security_token tokenData = container.STEPPY_API_t_security_token.Create();
            tokenData.id_user = newUser.id_user;
            tokenData.request_date = DateTime.Now;
            tokenData.expired_date = DateTime.Now.AddMonths(2);
            tokenData.is_logout = false;
            tokenData.security_token = Guid.NewGuid().ToString();
            container.STEPPY_API_t_security_token.Add(tokenData);
            container.SaveChanges();

            // create user profile
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

            //create user data to be transferred to cient
            UserDataDTO userdto = new UserDataDTO()
            {
                IdUser = newUser.id_user, 
                IdUserShesop = newUser.id_user_shesop,
                Email = newUserProfile.UserName,
                DisplayName = newUser.display_name,
                TelpNumber = newUser.telp_number,
                Gender = newUserProfile.JenisKelamin,
                Age = newUserProfile.Umur,
                Weight = newUserProfile.BeratBadan,
                Height = newUserProfile.Tinggi,
                JoinDate = newUser.join_date,
                Token = tokenData.security_token,
                /*UserContacts = container.STEPPY_API_m_contact
                                .Select(c => new UserContactDTO()
                                {
                                    IdContact = c.id_contact,
                                    TelpNumber = c.telp_number
                                }).ToList(),*/
                FriendProfiles = frienddto, 
                Profile = new ProfileDTO()
                {
                    IdUser = newProfile.id_user,
                    IdProfile = newProfile.id_profile,
                    DisplayName = newUser.display_name,
                    HiScore = newProfile.hi_score,
                    CurrentLevel = newProfile.current_level,
                    CurrentExperience = newProfile.current_experience,
                    NextLevelExperience = newProfile.next_level_experience,
                    Gold = newProfile.gold,
                    CurrentScore = newProfile.current_score,
                    Diamond = newProfile.diamond
                },
                FriendsUrl = Url.Link("TransactApi", new {  controller = "friend", token = tokenData.security_token }),
                ProfileUrl = Url.Link("TransactApi", new { controller = "profile", token = tokenData.security_token }),
                
            };

            /*
            //update registration status of this new user in contact info
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@telp_number", newUser.telp_number);
            sqlParams[1] = new SqlParameter("@countrycode", countryData.country_phone_prefix);
            int effected = container.Database.ExecuteSqlCommand("UPDATE STEPPY_API_m_contact SET is_registered = 1 WHERE telp_number = '0' + @telp_number OR telp_number = @countrycode + @telp_number",
                                                                 sqlParams);*/

            var response = Request.CreateResponse<UserDataDTO>(HttpStatusCode.Created, userdto);

            return response;
        }

        private string HashingPassword(string password)
        {
            var hashedPassword = Crypto.HashPassword(password);
            bool tr = Crypto.VerifyHashedPassword(hashedPassword, password);

            return hashedPassword;
        }
    }
}
