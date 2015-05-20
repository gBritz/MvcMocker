using System;
using System.Web.Mvc;

namespace MvcMocker.Test
{
    public class HomeController : Controller
    {
        public String GetUserName()
        {
            return (String)Session["userName"];
        }

        public Int32 CountSellingAttempt()
        {
            var result = 0;

            if (Session["user"] != null)
                result = (Int32)Session["countSellingAttempt"];

            return result;
        }
    }
}