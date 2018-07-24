﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SteppyNetAPI.WebAPI.Models;
using System.Diagnostics;
using System.Globalization;
using System.Web.Script.Serialization;
using SteppyNetAPI.WebAPI.Class;

namespace SteppyNetAPI.WebAPI.Controllers
{
    public class StepController : ApiController
    {
        STEPPY_APIContainer container = new STEPPY_APIContainer();

        // POST api/step
        public HttpResponseMessage Post(StepDTO value)
        {
            DateTime pDate = DateTime.ParseExact(value.Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int idShesop = int.Parse(value.IdUserShesop);
            STEPPY_new_record step = new STEPPY_new_record()
            {
                UserID = int.Parse(value.UserId),
                tanggal = DateTime.ParseExact(value.Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                jam_mulai = DateTime.ParseExact(value.StartTime, "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay,
                jam_akhir = DateTime.ParseExact(value.EndTime, "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay,
                step = int.Parse(value.Step),
                kalori = int.Parse(value.Calori),
                jenis_sensor = value.Sensor,
                user_id_shesop = idShesop,
                distance = decimal.Parse(value.Distance)
            };

            container.STEPPY_new_record.Add(step);
            container.SaveChanges();

            var response = Request.CreateResponse(HttpStatusCode.Created, container.STEPPY_new_record.Where(s => s.user_id_shesop == idShesop).Where(s => s.tanggal == pDate).ToList().Last());

            return response;
        }

        // GET api/step/
        public HttpResponseMessage Get(string date, int periode, string idShesop)
        {
           //Debug.WriteLine(_date + " " + periode);
            DateTime[] datesOfWeek;
            int[] steps;
            int id_shesop = int.Parse(idShesop);
            switch (periode)
            {
                case 1: //weekly data
                    List<int> lst = new List<int>();
                    steps = new int[7]; // 7: seven days of week

                    DateTime _date = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    int nWeekOfMonth = DateTimeHelper.GetWeekOfMonth(_date); //total week in month
                    DateTime[,] dates = DateTimeHelper.GroupDateByWeekOfMonth(_date); //collect date for each week of month
                    //Debug.WriteLine("1. " + dates + " " + nWeekOfMonth);
                    datesOfWeek = DateTimeHelper.GetDateOfWeek(dates, nWeekOfMonth); //get date of specific week of month
                    
                    for (int i = 0; i < datesOfWeek.Length; i++)
                    {
                        if (!datesOfWeek[i].Equals(new DateTime(0001, 1, 1))) //tidak dicetak jika null
                        {
                            //Debug.WriteLine(datesOfWeek[i] + " " + i);
                            for (int j = 0; j < 7; j++)
                            {
                                if (j == i)
                                {
                                    //Debug.WriteLine(datesOfWeek[i] + " " + i);

                                    DateTime dt = new DateTime(datesOfWeek[i].Year, datesOfWeek[i].Month, datesOfWeek[i].Day);
                                    DateTime nextdt = dt.AddDays(1);
                                    var dailyStep = container.STEPPY_new_record.Where<STEPPY_new_record>(s => s.user_id_shesop == id_shesop).Where(s => s.tanggal >= dt.Date && s.tanggal < nextdt.Date)
                                        .GroupBy(s=> s.tanggal)
                                        .Select(g => new
                                        {
                                            Sum = g.Select(s => s.step).Sum()
                                        }).ToList();
                                                                           
                                    if (dailyStep.Count > 0)
                                    steps[j] = (int)dailyStep.First().Sum;
                                }
                            }                          
                        }
                    }
                    break;
                case 2: // monthly 
                    DateTime date1 = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime[,] arrayofdates = DateTimeHelper.GroupDateByWeekOfMonth(date1);
                    DateTime firstd, lastd;

                    int weeks = DateTimeHelper.GetWeekOfMonth(new DateTime(arrayofdates[0, 0].Year, arrayofdates[0, 0].Month, DateTime.DaysInMonth(arrayofdates[0, 0].Year, arrayofdates[0, 0].Month)));
                    steps = new int[weeks];
                    for (int c = 0; c < weeks; c++)
                    {
                        //Debug.WriteLine("week - " + (c + 1));
                        firstd = arrayofdates[c, 0];
                        lastd = arrayofdates[c, 0];
                        for (int d = 0; d < 7; d++)
                        {
                           if (!arrayofdates[c, d].Equals(new DateTime(0001, 1, 1))) //tidak dicetak jika null
                            {
                                lastd = arrayofdates[c, d];
                                //Debug.WriteLine(arrayofdates[c, d]);
                            }
                        }
                        //Debug.WriteLine("tt = >" + firstd + " s.d " + lastd);
                        //query
                        var a = container.STEPPY_new_record.Where<STEPPY_new_record>(s => s.user_id_shesop == id_shesop).Where(s => s.tanggal >= firstd.Date && s.tanggal <= lastd.Date)
                            .GroupBy(s => s.tanggal.Value.Month)
                            .Select(g => new { 
                                Sum = g.Select(s=> s.step).Sum()
                            })
                            .ToList();
                        if(a.Count > 0)
                            steps[c] = (int)a.First().Sum;
                        //Debug.WriteLine("tt = >" + firstd + " s.d " + lastd);
                    }  
                    break;
                case 3: //yearly data
                    steps = new int[12];
                    DateTime first = new DateTime(DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Year, 1, 1);
                    DateTime last = first.AddYears(1);
                    
                    //query
                    var monthlyStep = container.STEPPY_new_record.Where<STEPPY_new_record>(s => s.user_id_shesop == id_shesop).Where(s => s.tanggal >= first.Date && s.tanggal < last.Date)
                            .GroupBy(s => s.tanggal.Value.Month)
                            .Select(g => new
                            {
                                Month = g.Select(s => s.tanggal.Value.Month),
                                Sum = g.Select(s => s.step).Sum()
                            }).ToList();
                   if (monthlyStep.Count > 0) {
                       for(int i=0;i<monthlyStep.Count;i++){
                           for (int j = 0; j < 12; j++)
                           {
                               //Debug.Write("t => " + (int)monthlyStep[i].Month.First());
                               if ((int)monthlyStep[i].Month.First() == (j + 1))
                                  steps[j] = (int)monthlyStep[i].Sum;
                           }
                       }
                   }
                   break;
                case 4:
                   steps = new int[1];
                    DateTime pDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var dStep = container.STEPPY_new_record.Where<STEPPY_new_record>(s => s.user_id_shesop == id_shesop).Where(s => s.tanggal == pDate)
                        .GroupBy(s => s.tanggal)
                        .Select(g => new
                        {
                            Sum = g.Select(s => s.step).Sum()
                        }).ToList();
                    if (dStep.Count > 0)
                    {
                        steps[0] = (int)dStep.First().Sum;
                    }
                    
                    break;
                default:
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, steps);
        }
    }
}
