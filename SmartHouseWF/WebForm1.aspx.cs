using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartHouseWF
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void AddDevice(int type, int id)
        {
            Panel device = new Panel();
            Button deleteButton = new Button();
            deleteButton.Command += Delete;
            deleteButton.ID = id.ToString();
            deleteButton.Text = "delete!";
            //deleteButton.OnClientClick = "alert('куку')";

            device.Controls.Add(deleteButton);

            switch (type)
            {
                case 1: device.Controls.Add(new RadioButton());
                    break;
                case 2: device.Controls.Add(new CheckBox());
                    break;
                case 3: device.Controls.Add(new TextBox());
                    break;
            }
            Panel1.Controls.Add(device);
        }

        protected void Delete(object sender, EventArgs e)
        {
            int id = Int32.Parse(((Button)sender).ID);
            ((List<int>)Session["Panel"]).RemoveAt(id);
            Response.Write(id);
            Response.Write("lalala");
            Draw();
        }

        protected void Draw()
        {
            Panel1.Controls.Clear();
            List<int> panelInfo = (List<int>)Session["Panel"];
            for (int i = 0; i < panelInfo.Count; i++)
            {
                int elTypeId = panelInfo[i];
                AddDevice(elTypeId, i);
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            ((List<int>)Session["Panel"]).Add(1);
            Draw();
            //AddDevice(1, ((List<int>)Session["Panel"]).Count - 1);
            //Panel1.Controls.Add(new RadioButton());

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            ((List<int>)Session["Panel"]).Add(2);
            Draw();
            //AddDevice(2, ((List<int>)Session["Panel"]).Count - 1);
            //Panel1.Controls.Add(new CheckBox());
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            ((List<int>)Session["Panel"]).Add(3);
            Draw();
            //AddDevice(3, ((List<int>)Session["Panel"]).Count - 1);
            //Panel1.Controls.Add(new TextBox());
        }
    }
}