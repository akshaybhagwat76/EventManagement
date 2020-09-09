using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class MyMoneyViewModel
    {
        public MyMoney MyMoney { get; set; }


        public decimal Balance{ get; set; }
        public MyMoneyViewModel() { }

        public MyMoneyViewModel(MyMoney MyMoney) {

            this.Balance = MyMoneyRepository.GetCalculatedBalance(MyMoney.ID);
            this.MyMoney = MyMoney;
        }

    }
}