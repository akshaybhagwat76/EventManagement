using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MiidWeb.Helpers
{
    public static class JsonCleaner
    {
        public static string RemoveSpaces(string input)
        {
            string spacesgone = Regex.Replace(input, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

            spacesgone = spacesgone.Replace("EventID", "EID");
            spacesgone = spacesgone.Replace("Price", "PRC");
            spacesgone = spacesgone.Replace("TicketCount", "TCT");
            spacesgone = spacesgone.Replace("TotalCost", "TTC");
            spacesgone = spacesgone.Replace("ChosenRowSeats", "CRS");
            spacesgone = spacesgone.Replace("TicketClassID", "TIC");
            spacesgone = spacesgone.Replace("TicketClassSeatID", "TCS");
            spacesgone = spacesgone.Replace("SeatNumber", "SNR");
            spacesgone = spacesgone.Replace("RowID", "RID");
            spacesgone = spacesgone.Replace("%0d%0a", "");
            spacesgone = spacesgone.Replace("TicketClassSeatRanges", "TCSR");
            

            return spacesgone;
        }
        public static string RestoreNames(string input)
        {
            string spacesgone = Regex.Replace(input, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

            spacesgone = spacesgone.Replace("EID","EventID" );
            spacesgone = spacesgone.Replace("PRC", "Price");
            spacesgone = spacesgone.Replace("TCT", "TicketCount");
            spacesgone = spacesgone.Replace("TTC", "TotalCost");
            spacesgone = spacesgone.Replace("CRS", "ChosenRowSeats");
            spacesgone = spacesgone.Replace("TIC", "TicketClassID");
            spacesgone = spacesgone.Replace("TCS", "TicketClassSeatID");
            spacesgone = spacesgone.Replace("SNR", "SeatNumber");
            spacesgone = spacesgone.Replace("RID", "RowID");

            spacesgone = spacesgone.Replace("TCSR","TicketClassSeatRanges");



            return spacesgone;
        }

    }
}