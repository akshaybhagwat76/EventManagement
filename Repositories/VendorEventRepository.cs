using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;
using MiidWeb.Models;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace MiidWeb.Repositories
{
    public class VendorEventRepository
    {

        public List<VendorEventViewModel> GetAllVendorTransactions(int EventID)
        {
            List<VendorEventViewModel> result = new List<VendorEventViewModel>();
            List<Vendor> vendors = new List<Vendor>();

            using (MiidEntities db = new MiidEntities())
            {
                Event event1 = db.Events.Find(EventID);

                var vendorEvents = db.VendorEvents.Where(x => x.EventID == EventID);

                foreach (var ve in vendorEvents)
                {
                    vendors.Add(db.Vendors.Find(ve.VendorID));

                }

                foreach (var v in vendors)
                {

                    var posTransactions = db.POSTransactions.Where(x => x.EventID == EventID && x.VendorID == v.ID);

                    VendorEventViewModel model = new VendorEventViewModel
                    {

                        Event = event1,
                        Vendor = v,
                        POSTransactions = posTransactions.ToList()

                    };

                    result.Add(model);
                }

            }

            return result;

        }

        public List<VendorReportModel> GetSingleVendorTransactionsForEvent(int EventID, int VendorID)
        {


            List<VendorReportModel> result = new List<VendorReportModel>();

            List<POSTransaction> poses = new List<POSTransaction>();

            using (MiidEntities db = new MiidEntities())
            {

                poses = db.POSTransactions.Where(x => x.EventID == EventID && x.VendorID == VendorID).OrderBy(x => x.EndTime).ToList();

                foreach (var pos in poses)
                {

                    var vendor = db.Vendors.Find(pos.VendorID);

                    VendorReportModel model = new VendorReportModel
                    {

                        Amount = (decimal)(pos.Amount??0.00M),
                        StaffName = db.Staffs.Find(pos.StaffID).CurrentStaffCode?? db.Staffs.Find(pos.StaffID).Code,
                        DeviceCode = db.Devices.Find(pos.DeviceID).DeviceCode,
                        EndTime = (DateTime)(pos.EndTime),
                        EventName = db.Events.Find(pos.EventID).EventName,
                        ID = pos.ID,
                        VendorName = vendor.Name,
                        VendorCode = vendor.VendorCode,
                        VendorContactName = vendor.ContactName,
                        ShiftNo = (int)pos.ShiftNo,
                        VendorEmail = vendor.Email,
                        VendorTelephone = vendor.Telephone
                    };

                    result.Add(model);
                }

            }

            return result;

        }


        //public List<VendorReportModel> GetSingleVendorTransactionsForEvent(int EventID, int VendorID)
        //{


        //    List<VendorReportModel> result = new List<VendorReportModel>();

        //    List<POSTransaction> poses = new List<POSTransaction>();

        //    using (MiidEntities db = new MiidEntities())
        //    {

        //        poses = db.POSTransactions.Where(x => x.EventID == EventID && x.VendorID == VendorID).OrderBy(x => x.EndTime).ToList();

        //        foreach (var pos in poses)
        //        {

        //            var vendor = db.Vendors.Find(pos.VendorID);

        //            VendorReportModel model = new VendorReportModel
        //            {

        //                Amount = (decimal)(pos.Amount),
        //                StaffName = db.Staffs.Find(pos.StaffID).Code,
        //                DeviceCode = db.Devices.Find(pos.DeviceID).DeviceCode,
        //                EndTime = (DateTime)(pos.EndTime),
        //                EventName = db.Events.Find(pos.EventID).EventName,
        //                ID = pos.ID,
        //                VendorName = vendor.Name,
        //                VendorCode = vendor.VendorCode,
        //                VendorContactName = vendor.ContactName,
        //                ShiftNo = (int)pos.ShiftNo,
        //                VendorEmail = vendor.Email,
        //                VendorTelephone = vendor.Telephone
        //            };

        //            result.Add(model);
        //        }

        //    }

        //    return result;

        //}


        public  List<VendorSummaryReportModel> ReportVendorSummary(int EventID, int VendorID)
        {
            List<VendorSummaryReportModel> reportData = new List<VendorSummaryReportModel>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("ReportVendorSummary", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;

            sqlDataAdapter.SelectCommand.Parameters.Add("@VendorID", SqlDbType.Int).Value = VendorID;

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    var VendorSummaryReportModel = new VendorSummaryReportModel
                    {
                        VendorName = row["VendorName"].ToString(),
                        //ShiftStartTime = DateTime.Parse(row["ShiftStart"].ToString()),
                        //ShiftEndTime = DateTime.Parse(row["ShiftEnd"].ToString()),
                        Amount = decimal.Parse(row["Amount"].ToString()),
                        ShiftNo = int.Parse(row["ShiftNo"].ToString()),
                        EventName = row["EventName"].ToString(),
                        VendorCode = row["VendorCode"].ToString(),
                        VendorContactName = row["VendorContactName"].ToString(),
                        VendorEmail = row["VendorEmail"].ToString(),
                        VendorTelephone = row["VendorTelephone"].ToString()
                        
                    };

                    //CONTINUED


                    DateTime start; DateTime end;
                    DateTime? start1 = null; DateTime? end1 = null;
                    if (DateTime.TryParse(row["ShiftStart"].ToString(), out start))
                    {
                        start1 = start;
                        VendorSummaryReportModel.ShiftStartTime = start;
                    }
                    if (DateTime.TryParse(row["ShiftEnd"].ToString(), out end))
                    {
                        VendorSummaryReportModel.ShiftEndTime = end;
                        end1 = end;
                    }


                    reportData.Add(VendorSummaryReportModel);
                }
            }
            sqlConnection.Close();
            return reportData;
        }




        public decimal GetAllVendorTransactionsTotal(int EventID)
        {

            using (MiidEntities db = new MiidEntities())
            {
                var posTransactions = db.POSTransactions.Where(x => x.EventID == EventID);

                if (posTransactions != null && posTransactions.Count() > 0)
                {
                    return posTransactions.Sum(y => (decimal)(y.Amount ?? 0));
                }
                else
                {
                    return 0.00M;
                }
            }
        }

        public List<Vendor> GetVendorsForEvent(int EventID)
        {
            List<Vendor> vendors = new List<Vendor>();

            using (MiidEntities db = new MiidEntities())
            {
                Event event1 = db.Events.Find(EventID);

                var vendorEvents = db.VendorEvents.Where(x => x.EventID == EventID);

                foreach (var ve in vendorEvents)
                {
                    vendors.Add(db.Vendors.Find(ve.VendorID));

                }


            }

            return vendors;
        }
    }
}