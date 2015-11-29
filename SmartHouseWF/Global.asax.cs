using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SmartHouseWF
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            Session["Panel"] = new List<int>();
            // Положение экрана со времени последнего запроса храниться в куках.
            Response.Cookies["scrollTop"].Value = 0.ToString();
        }
    }
}