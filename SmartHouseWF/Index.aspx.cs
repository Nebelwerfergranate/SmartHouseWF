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

            // Device
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

            // IClock
            if (device is IClock)
            {
                if (e.CommandName == "SetTime")
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

            // IOpenable
            if (device is IOpenable)
            {
                if (e.CommandName == "Open")
                {
                    ((IOpenable)device).Open();
                }
                else if (e.CommandName == "Close")
                {
                    ((IOpenable)device).Close();
                }
            }

            // ITemperature
            if (device is ITemperature)
            {
                if (e.CommandName == "SetTemperature")
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

            // ITimer
            if (device is ITimer)
            {
                if (e.CommandName == "SetTimer")
                {
                    string userSecondsInput = ((TextBox)e.Item.FindControl("TimerSeconds")).Text;
                    string userMinutesInput = ((TextBox)e.Item.FindControl("TimerMinutes")).Text;
                    string userHoursInput = ((TextBox)e.Item.FindControl("TimerHours")).Text;

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

                    int hours = 0;
                    if (userHoursInput != "")
                    {
                        if (!validator.GetHours(userHoursInput, out hours))
                        {
                            return;
                        }
                    }

                    ((ITimer)device).SetTimer(new TimeSpan(hours, minutes, seconds));
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

            // Fridge
            if (device is Fridge)
            {
                // Coldstore
                if (e.CommandName == "OpenColdstore")
                {
                    ((Fridge)device).OpenColdstore();
                }
                else if (e.CommandName == "CloseColdstore")
                {
                    ((Fridge)device).CloseColdstore();
                }
                else if (e.CommandName == "ColdstoreSetTemperature")
                {
                    string userInput = ((TextBox)e.Item.FindControl("ColdstoreTemperatureTextBox")).Text;

                    int temperature = 0;
                    if (!validator.GetTemperature(userInput, out temperature))
                    {
                        return;
                    }

                    ((Fridge)device).ColdstoreTemperature = temperature;
                }

                // Freezer
                else if (e.CommandName == "OpenFreezer")
                {
                    ((Fridge)device).OpenFreezer();
                }
                else if (e.CommandName == "CloseFreezer")
                {
                    ((Fridge)device).CloseFreezer();
                }
                else if (e.CommandName == "FreezerSetTemperature")
                {
                    string userInput = ((TextBox)e.Item.FindControl("FreezerTemperatureTextBox")).Text;

                    int temperature = 0;
                    if (!validator.GetTemperature(userInput, out temperature))
                    {
                        return;
                    }

                    ((Fridge)device).FreezerTemperature = temperature;
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
                    DateTime curTime = ((IClock)device).CurrentTime;
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
                ((Panel)e.Item.FindControl("ITemperaturePanel")).Visible = true;
                TextBox TemperatureTextBox = (TextBox)e.Item.FindControl("TemperatureTextBox");
                TemperatureTextBox.Text = ((ITemperature)device).Temperature.ToString();
                ((HiddenField)e.Item.FindControl("MinTemperature")).Value = ((ITemperature)device).MinTemperature.ToString();
                ((HiddenField)e.Item.FindControl("MaxTemperature")).Value = ((ITemperature)device).MaxTemperature.ToString();
            }

            // ITimer
            if (device is ITimer)
            {
                Panel ITimerPanel = (Panel)e.Item.FindControl("ITimerPanel");
                ITimerPanel.Visible = true;

                Label TimerIsRunningLabel = (Label)e.Item.FindControl("TimerIsRunning");
                if (((ITimer)device).IsRunning)
                {
                    TimerIsRunningLabel.Text = "Device is running";
                }
                else
                {
                    TimerIsRunningLabel.Text = "Device not running";
                }

                if (device.IsOn)
                {
                    ((Panel)e.Item.FindControl("ITimerDynamicPanel")).Visible = true;

                    TextBox hours = (TextBox)e.Item.FindControl("TimerHours");
                    hours.Text = "";

                    TextBox minutes = (TextBox)e.Item.FindControl("TimerMinutes");
                    minutes.Text = "";

                    TextBox seconds = (TextBox)e.Item.FindControl("TimerSeconds");
                    seconds.Text = "";
                }
            }

            // IVolume
            if (device is IVolume)
            {
                ((Panel)e.Item.FindControl("IVolumePanel")).Visible = true;
                ((Label)e.Item.FindControl("VolumeLabel")).Text =
                    "Volume: " + ((IVolume)device).Volume + " l";
            }

            // Fridge
            if (device is Fridge)
            {
                ((Panel)e.Item.FindControl("FridgePanel")).Visible = true;

                // Coldstore
                //+ ColdstoreIsOpenLabel
                //+ ColdstoreTemperatureTextBox
                //+ ColdstoreMinTemperature
                //+ ColdstoreMaxTemperature
                //+ ColdstoreIsHighlightedLabel
                //+ ColdstoreVolumeLabel
                Label coldstoreIsOpenLabel = (Label)e.Item.FindControl("ColdstoreIsOpenLabel");
                if (((Fridge)device).ColdstoreIsOpen)
                {
                    coldstoreIsOpenLabel.Text = "Coldstore is open";
                }
                else
                {
                    coldstoreIsOpenLabel.Text = "Coldstore is closed";
                }

                TextBox coldstoreTemperatureTextBox = (TextBox)e.Item.FindControl("ColdstoreTemperatureTextBox");
                coldstoreTemperatureTextBox.Text = ((Fridge)device).ColdstoreTemperature.ToString();
                ((HiddenField)e.Item.FindControl("ColdstoreMinTemperature")).Value = ((Fridge)device).ColdstoreMinTemperature.ToString();
                ((HiddenField)e.Item.FindControl("ColdstoreMaxTemperature")).Value = ((Fridge)device).ColdstoreMaxTemperature.ToString();

                Label coldstoreIsHighlightedLabel = (Label)e.Item.FindControl("ColdstoreIsHighlightedLabel");
                if (((Fridge)device).ColdstoreIsHighlighted)
                {
                    coldstoreIsHighlightedLabel.Text = "Coldstore backlight is on";
                }
                else
                {
                    coldstoreIsHighlightedLabel.Text = "Coldstore backlight is off";
                }

                ((Label)e.Item.FindControl("ColdstoreVolumeLabel")).Text =
                    "Coldstore volume: " + ((Fridge)device).ColdstoreVolume + " l";

                // Freezer
                //+ RefrigeratoryIsOpenLabel
                //+ RefrigeratoryTemperatureTextBox
                //+ RefrigeratoryMinTemperature
                //+ RefrigeratoryMaxTemperature
                //+ RefrigeratoryVolumeLabel
                Label refrigeratoryIsOpenLabel = (Label)e.Item.FindControl("FreezerIsOpenLabel");
                if (((Fridge)device).FreezerIsOpen)
                {
                    refrigeratoryIsOpenLabel.Text = "Freezer is open";
                }
                else
                {
                    refrigeratoryIsOpenLabel.Text = "Freezer is closed";
                }

                TextBox refrigeratoryTemperatureTextBox = (TextBox)e.Item.FindControl("FreezerTemperatureTextBox");
                refrigeratoryTemperatureTextBox.Text = ((Fridge)device).FreezerTemperature.ToString();
                ((HiddenField)e.Item.FindControl("FreezerMinTemperature")).Value = ((Fridge)device).FreezerMinTemperature.ToString();
                ((HiddenField)e.Item.FindControl("FreezerMaxTemperature")).Value = ((Fridge)device).FreezerMaxTemperature.ToString();

                ((Label)e.Item.FindControl("FreezerVolumeLabel")).Text =
                    "Freezer volume: " + ((Fridge)device).FreezeryVolume + " l";
            }

            // Specific
            if (device is Microwave)
            {
                ((TextBox)e.Item.FindControl("TimerHours")).Visible = false;
                ((HtmlGenericControl)e.Item.FindControl("TimerHoursSeparator")).Visible = false;
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
            else if (Microwave.Checked)
            {
                Messanger.Text = deviceManager.AddMicrowave(name);
            }
            else if (Fridge.Checked)
            {
                Messanger.Text = deviceManager.AddFridge(name);
            }
            else if (SomethingElseRadio.Checked)
            {
                Messanger.Text = "Что-то куда-то было добавлено...";
            }
        }
    }
}