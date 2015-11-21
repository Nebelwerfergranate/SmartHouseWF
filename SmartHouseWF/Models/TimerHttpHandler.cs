using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartHouseWF.Models
{
    public class TimerHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            System.Threading.Thread.Sleep(10000);
            context.Response.Write("ok");
        }
    }
}