using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SmartHouse;
using SmartHouseWF.Models;
using SmartHouseWF.Models.DeviceManager;

namespace SmartHouseWF
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private SessionDeviceManager deviceManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                deviceManager = new SessionDeviceManager(true);
            }
            else
            {
                deviceManager = new SessionDeviceManager(false);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Repeater1.DataSource = deviceManager.GetDevices();
            Repeater1.DataBind();
        }

        protected void OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            TimeInputValidator validator = new TimeInputValidator();

            uint id;
            UInt32.TryParse(((HiddenField) e.Item.FindControl("DeviceID")).Value, out id);
            
            Device device = deviceManager.GetDeviceById(id);
            if (e.CommandName == "Remove")
            {
                deviceManager.RemoveById(id);
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
                    
                    int hours = 0;
                    if (!validator.GetHours(userHoursInput, out hours))
                    {
                        return;
                    }

                    int minutes = 0;
                    if (!validator.GetMinutes(userMinutesInput, out minutes))
                    {
                        return;
                    }
                    
                    ((IClock)device).CurrentTime = new DateTime(1, 1, 1, hours, minutes, 0);
                }
            }

            // ITimer
            else if (e.CommandName == "SetTimer")
            {
                if (device is ITimer)
                {
                    string userSecondsInput = ((TextBox)e.Item.FindControl("TimerSeconds")).Text;
                    string userMinutesInput = ((TextBox)e.Item.FindControl("TimerMinutes")).Text;

                    int seconds = 0;
                    if (!validator.GetSeconds(userSecondsInput, out seconds))
                    {
                        return;
                    }

                    int minutes = 0;
                    if (!validator.GetMinutes(userMinutesInput, out minutes))
                    {
                        return;
                    }

                    ((ITimer) device).SetTimer(new TimeSpan(0, minutes, seconds));
                }   
            }
            else if (e.CommandName == "StartTimer")
            {
                ((ITimer)device).Start();
            }
            else if (e.CommandName == "StopTimer")
            {
                ((ITimer)device).Stop();
            }
        }

        protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            KeyValuePair<uint, Device> dataItem = (KeyValuePair<uint, Device>)e.Item.DataItem;
            Device device = dataItem.Value;

            uint id = dataItem.Key;
            ((HiddenField)e.Item.FindControl("DeviceID")).Value = id.ToString();
            
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

            // IClock
            if (device is IClock)
            {
                Panel currentTimePanel = (Panel)e.Item.FindControl("CurrentTime");
                currentTimePanel.Visible = true;
                HiddenField hillenField = (HiddenField)e.Item.FindControl("HiddenTimestamp");
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

                    Label timeSeparator = (Label)e.Item.FindControl("ClockTimeSeparator");
                    timeSeparator.Visible = true;
                    
                    TextBox minutes = (TextBox)e.Item.FindControl("Minutes");
                    minutes.Visible = true;
                    minutes.Text = "";
                    
                    ((Button)(e.Item.FindControl("SetTimeButton"))).Visible = true;
                }
            }

            // ITimer
            if (device is ITimer)
            {
                Panel ITimerPanel = (Panel)e.Item.FindControl("ITimerPanel");
                ITimerPanel.Visible = true;

                if (device.IsOn)
                {
                    ((Button)(e.Item.FindControl("StartButton"))).Visible = true;
                    ((Button)(e.Item.FindControl("StopButton"))).Visible = true;

                    Label TimerIsRunningLabel = (Label)e.Item.FindControl("TimerIsRunning");
                    TimerIsRunningLabel.Visible = true;
                    if (((ITimer) device).IsRunning)
                    {
                        TimerIsRunningLabel.Text = "Device is running";
                    }
                    else
                    {
                        TimerIsRunningLabel.Text = "Device not running";
                    }

                    TextBox hours = (TextBox)e.Item.FindControl("TimerMinutes");
                    hours.Visible = true;
                    hours.Text = "";

                    Label timeSeparator = (Label)e.Item.FindControl("TimerTimeSeparator");
                    timeSeparator.Visible = true;

                    TextBox minutes = (TextBox)e.Item.FindControl("TimerSeconds");
                    minutes.Visible = true;
                    minutes.Text = "";

                    ((Button)(e.Item.FindControl("SetTimerButton"))).Visible = true;
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
            else if (Oven.Checked)
            {
                Messanger.Text = deviceManager.AddOven(name);
            }
            else if (SomethingElseRadio.Checked)
            {
                Messanger.Text = "Что-то куда-то было добавлено...";
            }
        }
    }
}