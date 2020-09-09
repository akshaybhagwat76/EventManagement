using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Helpers
{
    public static class LogshipHelper
    {

        //public static int MaxIDForTable(string TableName)
        //{

        //    MiidEntities db = new MiidEntities();

        //    DateTime max = db.LogShippers.Max(x => x.ShipDate).Value;
        //    var logShip = db.LogShippers.Where(x => x.ShipDate == max).ToList();

        //    return (int)logShip.Where(x => x.TableName == TableName).First().LastMaxID;
        //}

        //public static DateTime MaxBatchDate(string TableName)
        //{

        //    MiidEntities db = new MiidEntities();

        //    DateTime max = db.LogShippers.Max(x => x.ShipDate).Value;

        //    return max;
        //}
    }
}