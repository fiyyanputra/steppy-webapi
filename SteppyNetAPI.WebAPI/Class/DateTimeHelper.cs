using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace SteppyNetAPI.WebAPI.Class
{
    public class DateTimeHelper
    {
        public static double DateToEpoch(DateTime date)
        {
            return (date - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static DateTime EpochToDate(long epoch)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(epoch);
        }

        //start with monday as 0
        public static int DayOfWeekToInt(DayOfWeek d)
        {
            if (d == DayOfWeek.Monday)
                return 0;
            if (d == DayOfWeek.Tuesday)
                return 1;
            if (d == DayOfWeek.Wednesday)
                return 2;
            if (d == DayOfWeek.Thursday)
                return 3;
            if (d == DayOfWeek.Friday)
                return 4;
            if (d == DayOfWeek.Saturday)
                return 5;
            if (d == DayOfWeek.Sunday)
                return 6;
            return -1;
        }

        public static DateTime GetFirstWeekDate(DateTime today)
        {
            return today.Date.Subtract(new TimeSpan(DayOfWeekToInt(today.DayOfWeek), 0, 0, 0));
        }

        public static double GetFirstWeekEpoch(DateTime today)
        {
            return DateToEpoch(GetFirstWeekDate(today));
        }

        //Helper to getting data for graphic Steppy purposes

        public static int GetWeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        public static int GetWeekOfMonth(DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return GetWeekOfYear(time) - GetWeekOfYear(first) + 1;
        }

        public static DateTime[,] GroupDateByWeekOfMonth(DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1); //mendapatkan tanggal awal dari param
            DateTime last = new DateTime(time.Year, time.Month, DateTime.DaysInMonth(time.Year, time.Month)); //mendapatkan tanggal terakhir 
            int maxWeek = GetWeekOfMonth(last); //mendapatkan jumlah minggu pada bulan
            //Debug.WriteLine(first.ToString("dd") + "=>" + last.ToString("dd"));

            DateTime[,] weekMembers = new DateTime[maxWeek, 7];

            int i = 0;
            int j;
            while (i < maxWeek - 1) //selama jumlah minggu dalam bulan
            {
                j = 0;
                while (first <= last) //selama jumlah tanggal dalam bulan
                {
                    if (GetWeekOfMonth(first) - 1 == i)
                    {
                        weekMembers[GetWeekOfMonth(first) - 1, j] = first;
                        j++;
                        first = first.AddDays(1);
                    }
                    else
                    {
                        i++;
                        j = 0;
                    }
                }
            }

            return weekMembers;
        }

        public static DateTime[] GetDateOfWeek(DateTime[,] array, int nWeekOfMonth)
        {
            DateTime[] dateofweek = new DateTime[7];

            for (int c = 0; c < GetWeekOfMonth(new DateTime(array[0, 0].Year, array[0, 0].Month, DateTime.DaysInMonth(array[0, 0].Year, array[0, 0].Month))); c++)
            {
                if (nWeekOfMonth == (c + 1))
                {
                    //Debug.WriteLine("week - " + (c + 1));
                    for (int d = 0; d < 7; d++)
                    {
                        if (!array[c, d].Equals(new DateTime(0001, 1, 1))) //tidak dicetak jika null
                        {
                            //Debug.WriteLine("x=> " + (int)array[c, d].DayOfWeek + " " + array[c, d]);
                            dateofweek[(int)array[c, d].DayOfWeek] = array[c, d];
                        }
                    }
                }
            }

            return dateofweek;
        }
    }
}