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
    public class DeviceRepository
    {

        public static Device GetByCode(string DeviceCode)
        {
            Device device = new Device();

            using (MiidEntities db = new MiidEntities())
            {
                

                var devices = db.Devices.Where(x => x.DeviceCode == DeviceCode);
                if (devices != null && devices.Count() > 0)
                {
                    device = devices.First();

                }
                else
                {
                    device = CreateNewDevice(DeviceCode);
                
                }
                

            }

            return device;

        }

    
        public static Device CreateNewDevice(string DeviceCode)
        {
            using (MiidEntities db = new MiidEntities())
            {

                Device device = new Device();
                device.DeviceCode = DeviceCode;
                device.DeviceAppCode = DeviceCode;
                device.DateTimeAdded = DateTime.Now;

                db.Devices.Add(device);
                db.SaveChanges();

                return device;

            }
        }
    }
}