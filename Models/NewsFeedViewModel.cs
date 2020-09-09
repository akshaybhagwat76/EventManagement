using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;

namespace MiidWeb.Models
{
    public class  NewsFeedViewModel
    {
        private MiidEntities db = new MiidEntities();
        public List<NewsFeed> NewsFeedList { get; set; }



        public int LoggedInUserID { get; set; }
        public int UserID { get; set; }

        public NewsFeedViewModel(int UserID, int Count, int loggedInUserID)
        {
        
            this.NewsFeedList = db.NewsFeeds.Include(x=>x.NewsFeedComments).Where(x => x.EndUserID == UserID).OrderByDescending(x=>x.DateCreated).Take(Count).ToList();
            this.LoggedInUserID = loggedInUserID;
            this.UserID = UserID;

        
        }
    }
}
