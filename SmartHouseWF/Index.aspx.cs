using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
            Device device = deviceManager.GetDeviceByName(name);
            if (e.CommandName == "Remove")
            {
                deviceManager.RemoveByName(name);
            }
            else if (e.CommandName == "Toggle")
            {

                if (device.IsOn)
                {
                    device.TurnOff();
                }
                else
                {
                    device.TurnOn();
                }
            }
            else if (e.CommandName == "SetTime")
            {
                if (device is IClock)
                {
                    string userHoursInput = ((TextBox) e.Item.FindControl("Hours")).Text;
                    string userMinutesInput = ((TextBox)e.Item.FindControl("Minutes")).Text;

                    Regex regex = new Regex("^[0-9]{1,2}$");
                    Match hoursMatch = regex.Match(userHoursInput);
                    Match minutesMatch = regex.Match(userMinutesInput);
       
                    if (!hoursMatch.Success || !minutesMatch.Success)
                    {
                        return;
                    }
                   
                    int hours = Int32.Parse(userHoursInput);
                    int minutes = Int32.Parse(userMinutesInput);
                    if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
                    {
                        return;
                    }
                    
                    ((IClock)device).CurrentTime = new DateTime(1, 1, 1, hours, minutes, 0);
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

                    TextBox hours = (TextBox)e.Item.FindControl("Hours");
                    hours.Visible = true;
                    hours.Text = "";

                    Label timeSeparator = (Label)e.Item.FindControl("TimeSeparator");
                    timeSeparator.Visible = true;
                    
                    TextBox minutes = (TextBox)e.Item.FindControl("Minutes");
                    minutes.Visible = true;
                    minutes.Text = "";
                    
                    ((Button)(e.Item.FindControl("SetTimeButton"))).Visible = true;
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