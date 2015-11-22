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
            Validator validator = new Validator();

            uint id;
            UInt32.TryParse(((HiddenField)e.Item.FindControl("DeviceID")).Value, out id);

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
                    string userHoursInput = ((TextBox)e.Item.FindControl("Hours")).Text;
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

                    ((ITimer)device).SetTimer(new TimeSpan(0, minutes, seconds));
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

            // IOpenable
            else if (e.CommandName == "Open")
            {
                ((IOpenable)device).Open();
            }
            else if (e.CommandName == "Close")
            {
                ((IOpenable)device).Close();
            }

            // ITemperature
            else if (e.CommandName == "SetTemperature")
            {
                string userInput = ((TextBox)e.Item.FindControl("TemperatureTextBox")).Text;

                int temperature = 0;
                if (!validator.GetTemperature(userInput, out temperature))
                {
                    return;
                }

                ((ITemperature)device).Temperature = temperature;
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
                    ((Panel)e.Item.FindControl("ITimerDynamicPanel")).Visible = true;

                    Label TimerIsRunningLabel = (Label)e.Item.FindControl("TimerIsRunning");
                    if (((ITimer)device).IsRunning)
                    {
                        TimerIsRunningLabel.Text = "Device is running";
                    }
                    else
                    {
                        TimerIsRunningLabel.Text = "Device not running";
                    }

                    TextBox hours = (TextBox) e.Item.FindControl("TimerHours");
                    hours.Text = "";

                    TextBox minutes = (TextBox)e.Item.FindControl("TimerMinutes");
                    minutes.Text = "";

                    TextBox seconds = (TextBox)e.Item.FindControl("TimerSeconds");
                    seconds.Text = "";
                }
            }

            // IBacklight
            if (device is IBacklight)
            {
                Panel IBacklightPanel = (Panel)e.Item.FindControl("IBacklightPanel");
                IBacklightPanel.Visible = true;

                Label IsHighlightedLabel = (Label)e.Item.FindControl("IsHighlightedLabel");
                if (((IBacklight)device).IsHighlighted)
                {
                    IsHighlightedLabel.Text = "Backlight is on";
                }
                else
                {
                    IsHighlightedLabel.Text = "Backlight is off";
                }
            }

            // IOpenable
            if (device is IOpenable)
            {
                Panel IOpenablePanel = (Panel)e.Item.FindControl("IOpenablePanel");
                IOpenablePanel.Visible = true;

                Label IsOpenLabel = (Label)e.Item.FindControl("IsOpenLabel");
                if (((IOpenable)device).IsOpen)
                {
                    IsOpenLabel.Text = "Device is open";
                }
                else
                {
                    IsOpenLabel.Text = "Device is closed";
                }
            }

            // ITemperature
            if (device is ITemperature)
            {
                ((Panel) e.Item.FindControl("ITemperaturePanel")).Visible = true;
                TextBox TemperatureTextBox = (TextBox) e.Item.FindControl("TemperatureTextBox");
                TemperatureTextBox.Text = ((ITemperature) device).Temperature.ToString();
                ((HiddenField)e.Item.FindControl("MinTemperature")).Value = ((ITemperature)device).MinTemperature.ToString();
                ((HiddenField)e.Item.FindControl("MaxTemperature")).Value = ((ITemperature)device).MaxTemperature.ToString();
            }

            // IVolume
            {
                if (device is IVolume)
                {
                   ((Panel) e.Item.FindControl("IVolumePanel")).Visible = true;
                    ((Label) e.Item.FindControl("VolumeLabel")).Text = "Volume: " +
                                                                       ((IVolume) device).Volume +
                                                                       " sm<sup>3</sup>";
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