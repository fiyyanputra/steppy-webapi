using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteppyNetAPI.WebAPI.Models
{
    public class FriendDataDTO
    {
        private int? _todayStep;
        private int? _weeklyStep;

        public int? FriendIdUser { get; set; }
        public string DisplayName { get; set; }
        public int IdContact { get; set; }
        public int? IdUser { get; set; }
        public string TelpNumber { get; set; }
        public string FriendDetailUrl { get; set; }
        public decimal? HiScore { get; set; }
        public int? Level { get; set; }
        public int? TodayStep
        {
            get
            {
                if (this._todayStep == null)
                    this._todayStep = 0;
                return this._todayStep;
            }
            set { this._todayStep = value; }
        }
        public int? WeeklyStep
        {
            get
            {
                if (this._weeklyStep == null)
                    this._weeklyStep = 0;
                return this._weeklyStep;
            }
            set { this._weeklyStep = value; }
        }
    }
}