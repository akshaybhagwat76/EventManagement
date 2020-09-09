using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class CalendarViewModel
    {
        public int EndUserID { get; set; }
        public List<CalendarItem> Calendar { get; set;}

        public DateTime FirstOfMonth { get; set; }

        public CalendarViewModel(int EndUserID, DateTime AnyDate, int PlusMonths)
        {

            this.EndUserID = EndUserID;

            var FirstOfMonth = new DateTime(AnyDate.Year, AnyDate.Month, 1);

            this.Calendar = new List<CalendarItem>();
            this.FirstOfMonth = FirstOfMonth;
            
            DateTime EndOfMonth = FirstOfMonth.AddMonths(1).AddDays(-1);

            DateTime FirstMonday = GetStartOfWeek(FirstOfMonth, DayOfWeek.Monday);

            DateTime LastSunday = GetNextWeekday(EndOfMonth, DayOfWeek.Sunday);


            

            //Lag
            while ((FirstOfMonth - FirstMonday).TotalDays > 0)
            {
                this.Calendar.Add(new CalendarItem { DayNo = "" });
                FirstMonday = FirstMonday.AddDays(1);
            
            }

            //Middle
            this.Calendar.AddRange(EndUserRepository.GetMyEvents(EndUserID, AnyDate, PlusMonths));

            /*while ((EndOfMonth - FirstOfMonth).TotalDays >= 0)
            {
                var CalItem = new CalendarItem();
                              
                this.Calendar.Add(new CalendarItem { DayNo = FirstOfMonth.Day.ToString(), CalendarDate = FirstOfMonth });
                FirstOfMonth = FirstOfMonth.AddDays(1);
            
            }*/


            //Lead
            while ((LastSunday - EndOfMonth).TotalDays >= 0)
            {
                this.Calendar.Add(new CalendarItem { DayNo = "" });
                LastSunday = LastSunday.AddDays(-1);

            }
        
        }

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static DateTime GetStartOfWeek(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToSubtract = - ((int)start.DayOfWeek - (int)day + 7) % 7;
            return start.AddDays(daysToSubtract);
        }

    
    }



    public class CalendarItem
    {

        public string DayNo { get; set; }
        public DateTime CalendarDate { get; set; }
        public int EventID { get; set; }
        public string ShortDescription { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int TicketCount { get; set; }
        public string ImageURL { get; set; }
        public string ImageAltText { get; set; }

        public string EventName { get; set; }
        public string LongDescription { get; set; }
    
    
    }
}