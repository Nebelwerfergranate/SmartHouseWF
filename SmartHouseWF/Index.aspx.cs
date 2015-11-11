using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmartHouse;
using SmartHouseWF.Models.DeviceManager;

namespace SmartHouseWF
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private DeviceManager deviceManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                deviceManager = new DeviceManager(new Device[0]);
                deviceManager.AddClock("myClock");
                Session["devices"] = deviceManager.GetDevices();
            }
            else
            {
                deviceManager = new DeviceManager((Device[])Session["devices"]);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Repeater1.DataSource = deviceManager.GetDevices();
            Repeater1.DataBind();
            Session["devices"] = deviceManager.GetDevices();
        }

        protected void OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string name = ((Label)e.Item.FindControl("Name")).Text;
            if (e.CommandName == "Remove")
            {
                deviceManager.RemoveByName(name);
            }
            else if (e.CommandName == "Toggle")
            {
                Device device = deviceManager.GetDeviceByName(name);
                if (device.IsOn)
                {
                    device.TurnOff();
                }
                else
                {
                    device.TurnOn();
                }
            }
        }

        protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            Device device = (Device)e.Item.DataItem;

            ((Label)e.Item.FindControl("Name")).Text = device.Name;

            Label stateLabel = (Label)e.Item.FindControl("State");
            if (device.IsOn)
            {
                stateLabel.Text = "Включен";
            }
            else
            {
                stateLabel.Text = "Выключен";
            }

            if (device is IClock)
            {
                //Label currentTimeLabel = (Pa) e.Item.FindControl("CurrentTime");
                //currentTimeLabel.Visible = true;
                //currentTimeLabel.CssClass = "TimeField";
                Panel currentTimePanel = (Panel)e.Item.FindControl("CurrentTime");
                currentTimePanel.Visible = true;
                HiddenField hillenField = (HiddenField)e.Item.FindControl("js_currentTime");
               
                if (!device.IsOn)
                {
                    hillenField.Value = "disabled";
                }
                else
                {
                    DateTime curTime = ((Clock)device).CurrentTime;
                    // Convert current time to miliseconds
                    hillenField.Value = (curTime.Hour * 60 * 60 * 1000 +
                                    curTime.Minute * 60 * 1000 +
                                    curTime.Second * 1000).ToString();
                }
            }
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            string name = Request.Form["newDeviceName"];
            if (name == "")
            {
                Messanger.Text = "Имя не должно быть пустым!";
                return;
            }
            if (ClockRadio.Checked)
            {
                Messanger.Text = deviceManager.AddClock(name);
            }
            else if (SomethingElseRadio.Checked)
            {
                Messanger.Text = "Что-то куда-то было добавлено...";
            }
        }
    }
}