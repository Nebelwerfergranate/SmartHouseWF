using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<int> panelInfo = (List<int>)Session["Panel"];
        foreach (int elTypeId in panelInfo)
        {
            switch (elTypeId)
            {
                case 1: Panel1.Controls.Add(new RadioButton());
                break;
                case 2: Panel1.Controls.Add(new CheckBox());
                break;
                case 3: Panel1.Controls.Add(new TextBox());
                break;
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ((List<int>)Session["Panel"]).Add(1);
        Panel1.Controls.Add(new RadioButton());
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        ((List<int>)Session["Panel"]).Add(2);
        Panel1.Controls.Add(new CheckBox());
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        ((List<int>)Session["Panel"]).Add(3);
        Panel1.Controls.Add(new TextBox());
    }
}