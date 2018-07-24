using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class UserDataDTO
    {
        public int IdUser { get; set; }
        public int IdUserShesop { get; set; }
        public string DisplayName { get; set; }
        public string TelpNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public int? Age { get; set; }
        public DateTime JoinDate { get; set; }
        public string Token { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public List<UserContactDTO> UserContacts { get; set; }
        public List<FriendDataDTO> FriendProfiles { get; set; }
        public string FriendsUrl { get; set; }
        public string ProfileUrl { get; set; }
        public ProfileDTO Profile { get; set; }
        public GameDataDTO UserGameData { get; set; }
    }
}