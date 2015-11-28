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
            deviceManager = new SessionDeviceManager();
            AddMicrowaveList.DataSource = deviceManager.GetMicrowaveNames();
            AddMicrowaveList.DataBind();
            AddOvenList.DataSource = deviceManager.GetOvenNames();
            AddOvenList.DataBind();
            AddFridgeList.DataSource = deviceManager.GetFridgeNames();
            AddFridgeList.DataBind();
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
            else if (e.CommandName == "Rename")
            {
                string newName = Request.Form["DeviceName"];
                deviceManager.RenameById(id, newName);
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
                if (e.CommandName == "OpenClose")
                {
                    if (((IOpenable) device).IsOpen)
                    {
                        ((IOpenable) device).Close();
                    }
                    else
                    {
                        ((IOpenable)device).Open();
                    }
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
                else if (e.CommandName == "StartStop")
                {
                    if (((ITimer) device).IsRunning)
                    {
                        ((ITimer)device).Stop();
                    }
                    else
                    {
                        ((ITimer)device).Start();
                    }
                }
            }

            // Fridge
            if (device is Fridge)
            {
                // Coldstore
                if (e.CommandName == "ColdstoreOpenClose")
                {
                    if (((Fridge) device).ColdstoreIsOpen)
                    {
                        ((Fridge) device).CloseColdstore();
                    }
                    else
                    {
                        ((Fridge)device).OpenColdstore();
                    }
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
                else if (e.CommandName == "FreezerOpenClose")
                {
                    if (((Fridge) device).FreezerIsOpen)
                    {
                        ((Fridge) device).CloseFreezer();
                    }
                    else
                    {
                        ((Fridge) device).OpenFreezer();
                    }
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
            // Обновление страницы браузера должно работать корректно...
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
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

            ((Label)e.Item.FindControl("Name")).Text = "Name: " + device.Name;

            ImageButton stateButton = (ImageButton)e.Item.FindControl("State");
            if (device.IsOn)
            {
                stateButton.ImageUrl = "Content/Images/on.png";
            }
            else
            {
                stateButton.ImageUrl = "Content/Images/off.png";
            }

            // IBacklight
            if (device is IBacklight)
            {
                Panel IBacklightPanel = (Panel)e.Item.FindControl("IBacklightPanel");
                IBacklightPanel.Visible = true;

                Image IsHighlightedImage = (Image)e.Item.FindControl("IsHighlightedImage");
                if (((IBacklight)device).IsHighlighted)
                {
                    IsHighlightedImage.ImageUrl = "/Content/Images/backlightOn.png";
                }
                else
                {
                    IsHighlightedImage.ImageUrl = "/Content/Images/backlightOff.png";
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

                ImageButton IOpenableButton = (ImageButton)e.Item.FindControl("OpenCloseButton");
                if (((IOpenable)device).IsOpen)
                {
                    IOpenableButton.ImageUrl = "Content/Images/opened.png";
                }
                else
                {
                    IOpenableButton.ImageUrl = "Content/Images/closed.png";
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

                ImageButton startStopButton = (ImageButton)e.Item.FindControl("StartStopButton");
                if (((ITimer)device).IsRunning)
                {
                    startStopButton.ImageUrl = "Content/Images/stop.png";
                }
                else
                {
                    startStopButton.ImageUrl = "Content/Images/start.png";
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
                ImageButton coldstoreOpenCloseButton = (ImageButton)e.Item.FindControl("ColdstoreOpenCloseButton");
                if (((Fridge)device).ColdstoreIsOpen)
                {
                    coldstoreOpenCloseButton.ImageUrl = "Content/Images/opened.png";
                }
                else
                {
                    coldstoreOpenCloseButton.ImageUrl = "Content/Images/closed.png";
                }

                TextBox coldstoreTemperatureTextBox = (TextBox)e.Item.FindControl("ColdstoreTemperatureTextBox");
                coldstoreTemperatureTextBox.Text = ((Fridge)device).ColdstoreTemperature.ToString();
                ((HiddenField)e.Item.FindControl("ColdstoreMinTemperature")).Value = ((Fridge)device).ColdstoreMinTemperature.ToString();
                ((HiddenField)e.Item.FindControl("ColdstoreMaxTemperature")).Value = ((Fridge)device).ColdstoreMaxTemperature.ToString();

                Image coldstoreIsHighlighted = (Image)e.Item.FindControl("ColdstoreIsHighlighted");
                if (((Fridge)device).ColdstoreIsHighlighted)
                {
                    coldstoreIsHighlighted.ImageUrl = "/Content/Images/backlightOn.png";
                }
                else
                {
                    coldstoreIsHighlighted.ImageUrl = "/Content/Images/backlightOff.png";
                }

                ((Label)e.Item.FindControl("ColdstoreVolumeLabel")).Text =
                    "Coldstore volume: " + ((Fridge)device).ColdstoreVolume + " l";

                // Freezer
                ImageButton freezerOpenCloseButton = (ImageButton)e.Item.FindControl("FreezerOpenCloseButton");
                if (((Fridge)device).FreezerIsOpen)
                {
                    freezerOpenCloseButton.ImageUrl = "Content/Images/opened.png";
                }
                else
                {
                    freezerOpenCloseButton.ImageUrl = "Content/Images/closed.png";
                }

                TextBox refrigeratoryTemperatureTextBox = (TextBox)e.Item.FindControl("FreezerTemperatureTextBox");
                refrigeratoryTemperatureTextBox.Text = ((Fridge)device).FreezerTemperature.ToString();
                ((HiddenField)e.Item.FindControl("FreezerMinTemperature")).Value = ((Fridge)device).FreezerMinTemperature.ToString();
                ((HiddenField)e.Item.FindControl("FreezerMaxTemperature")).Value = ((Fridge)device).FreezerMaxTemperature.ToString();

                ((Label)e.Item.FindControl("FreezerVolumeLabel")).Text =
                    "Freezer volume: " + ((Fridge)device).FreezeryVolume + " l";
            }

            // Specific
            if (device is Clock)
            {
                ((Image)e.Item.FindControl("DeviceImage")).ImageUrl = "Content/Images/clock.png";
            }
            if (device is Microwave)
            {
                ((Image) e.Item.FindControl("DeviceImage")).ImageUrl = "Content/Images/microwave.png";
                ((TextBox)e.Item.FindControl("TimerHours")).Visible = false;
                ((HtmlGenericControl)e.Item.FindControl("TimerHoursSeparator")).Visible = false;
            }
            if (device is Oven)
            {
                ((Image)e.Item.FindControl("DeviceImage")).ImageUrl = "Content/Images/oven.png";
            }
            if (device is Fridge)
            {
                ((Image)e.Item.FindControl("DeviceImage")).ImageUrl = "Content/Images/fridge.png";
            }
        }

        protected void AddclockButton_OnClick(object sender, EventArgs e)
        {
            string name = Request.Form["DeviceName"];
            deviceManager.AddClock(name);
        }

        protected void AddMicrowaveButton_OnClick(object sender, EventArgs e)
        {
            string name = Request.Form["DeviceName"];
            string fabricator = Request.Form["AddMicrowaveList"];
            deviceManager.AddMicrowave(name, fabricator);
        }

        protected void AddOvenButton_OnClick(object sender, EventArgs e)
        {
            string name = Request.Form["DeviceName"];
            string fabricator = Request.Form["AddOvenList"];
            deviceManager.AddOven(name, fabricator);
        }

        protected void AddFridgeButton_OnClick(object sender, EventArgs e)
        {
            string name = Request.Form["DeviceName"];
            string fabricator = Request.Form["AddFridgeList"];
            deviceManager.AddFridge(name, fabricator);
        }
    }
}