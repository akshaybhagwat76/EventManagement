using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MiidWeb
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string con = "";
        public string querytouse = "";

        protected void Page_Load(object sender, EventArgs e)
        {



            if (!User.Identity.IsAuthenticated)
            {




                if (Request.QueryString != null && (Request.QueryString["query"] != null))
                {
                    if (Request.QueryString["secret"] == "miidsecretapi")
                    {

                        { con = ConfigRepo.Get("MiidConnectionString"); }
                        SelectQuery(Request.QueryString["query"], con);
                    }
                    else
                    {
                        Response.Redirect("Home");

                    }
                }
                else
                {
                    Response.Redirect("Home");

                }
            }
            else
            {
                if (!User.IsInRole("Admin"))
                {
                    Response.Redirect("Home");
                }

            }

          

			Button1.Attributes.Add("class", "btn btn-primary");
			TextBox1.Attributes.Add("class", "form-control");
		}

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            { con = ConfigRepo.Get("MiidConnectionString"); }
            else

            { con = ConfigRepo.Get("MiidConnectionString"); }
            SelectQuery(TextBox1.Text, con);

			
		}

        private void SelectQuery(string queryString, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                           connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        GridView1.DataSource = reader;
                        GridView1.DataBind();
						GridView1.Attributes.Add("class", "table table-bordered table-striped");
					}
                    else
                    {
                        Label1.Text = "Now rows found in table.";

                    }

                }
            }
            catch (Exception e)
            {
                Label1.Text = "Error:" + e.Message;

            }
        }


        private void ExecuteNon(string queryString, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                           connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();

                    Label1.Text = String.Format("Query result: {0}", command.ExecuteNonQuery().ToString());
                }
            }
            catch (Exception e)
            {
                Label1.Text = "Error:" + e.Message;

            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            { con = ConfigRepo.Get("MiidConnectionString"); }
            else

            { con = ConfigRepo.Get("MiidConnectionString"); }

            ExecuteNon(TextBox1.Text, con);

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "select * from sys.tables;";
        }

        protected void Button4_Click(object sender, EventArgs e)
        {

            string pathA = Server.MapPath("~/Uploads");
            string pathB = Server.MapPath("~/Thumbnails");

            System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(pathA);
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(pathB);

            // Take a snapshot of the file system.
            IEnumerable<System.IO.FileInfo> biglist = dir1.GetFiles("*.jpg", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> smalllist = dir2.GetFiles("*.jpg", System.IO.SearchOption.AllDirectories);

            //var biglist = Directory.EnumerateFiles(Server.MapPath("~/Uploads"), "*.jpg");
            //var smalllist = Directory.EnumerateFiles(Server.MapPath("~/Thumbnails"), "*.jpg");

            FileCompare myFileCompare = new FileCompare();

            var queryList1Only = (from file in biglist
                                  select file).Except(smalllist, myFileCompare);



            foreach (var file in queryList1Only)
            {
                //your code

                //int finalposition = file.LastIndexOf("\\");
                //string FileNameOnly = file.Substring(finalposition + 1, file.Length - (finalposition + 1));
                //var path = Path.Combine(Server.MapPath("~/Uploads"), file);

                string FileNameOnly = file.Name;
                string path = file.FullName;

                VaryQualityLevel(path, FileNameOnly);
            }

            //foreach (var file in Directory.EnumerateFiles(Server.MapPath("~/Uploads"), "*.jpg"))//Process all jpegs
            //{

            //    //your code
            //    int finalposition = file.LastIndexOf("\\");
            //    string FileNameOnly = file.Substring(finalposition + 1, file.Length - (finalposition +1));
            //    var path = Path.Combine(Server.MapPath("~/Uploads"), file);

            //    VaryQualityLevel(path, FileNameOnly);
            //}
        }





        private void VaryQualityLevel(string pathToFile, string fileName)
        {

            var newpath = Path.Combine(Server.MapPath("~/Thumbnails"), fileName);

            // Get a bitmap.
            Bitmap bmp1 = new Bitmap(pathToFile);
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
                50L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmp1.Save(newpath, jgpEncoder, myEncoderParameters);



        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }


    // This implementation defines a very simple comparison 
    // between two FileInfo objects. It only compares the name 
    // of the files being compared and their length in bytes. 
    public class FileCompare : System.Collections.Generic.IEqualityComparer<System.IO.FileInfo>
    {
        public FileCompare() { }

        public bool Equals(System.IO.FileInfo f1, System.IO.FileInfo f2)
        {
            return (f1.Name == f2.Name);// && f1.Length == f2.Length);
        }

        // Return a hash that reflects the comparison criteria. According to the  
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must 
        // also be equal. Because equality as defined here is a simple value equality, not 
        // reference identity, it is possible that two or more objects will produce the same 
        // hash code. 
        public int GetHashCode(System.IO.FileInfo fi)
        {
            string s = String.Format("{0}{1}", fi.Name, fi.Length);
            return s.GetHashCode();
        }
    }
}