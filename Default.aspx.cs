using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    public object currentItem = "nanana";

    protected void Page_Load(object sender, EventArgs e)
    {
        Repeater1.DataSource = Session["Panel"];
        DataBind();
        //Draw();
    }

    protected void AddDevice(int type, int id)
    {
        Panel device = new Panel();
        Button deleteButton = new Button();
        deleteButton.ID = id.ToString();
        deleteButton.Text = "delete!";
        deleteButton.Command += Delete;
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
        //Draw();
        AddDevice(1, ((List<int>)Session["Panel"]).Count - 1);
        //Panel1.Controls.Add(new RadioButton());
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        ((List<int>)Session["Panel"]).Add(2);
        //Draw();
        AddDevice(2, ((List<int>)Session["Panel"]).Count - 1);
        //Panel1.Controls.Add(new CheckBox());
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        ((List<int>)Session["Panel"]).Add(3);
        //Draw();
        AddDevice(3, ((List<int>)Session["Panel"]).Count - 1);
        //Panel1.Controls.Add(new TextBox());
    }
    protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        currentItem = e.Item.DataItem;
        Response.Write(e.Item.DataItem);
    }
}