using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Helpers
{
  public static class DateHelper
  {

    public static bool DateSequenceViolation(DateTime Today, DateTime Tomorrow)
    {
      TimeSpan timespan = (Tomorrow - Today);
      return timespan.TotalDays < 0;
    }




    public static SelectList MonthList(DateTime Start, int ListLength)
    {

      var list = new List<SelectListItem>();
      var start = new SelectListItem() { Value = Start.ToString("yyyy-MM-dd"), Text = Start.ToString("MMM-yyyy") };

      list.Add(start);
      int x = 0;
      DateTime date = Start.AddMonths(1);

      while (x < ListLength)
      {

        list.Add(new SelectListItem() { Value = date.ToString("yyyy-MM-dd"), Text = date.ToString("MMM-yyyy") });
        date = date.AddMonths(1);
        x++;
      }
      var list2 = new SelectList(list, "Value", "Text");

      return list2;
    }

    public static DateTime? CreateDate(int year, int month, int day, int hour, int min, int sec)
    {

      DateTime? returnDate = null;

      try
      {
        var datestring = new DateTime(year, month, day, hour, min, sec).ToString("yyy-MM-dd HH:mm");
        returnDate =  DateTime.Parse(datestring);
      }
      catch (Exception ex)
      {

      }


      return returnDate;
    }

  }
}