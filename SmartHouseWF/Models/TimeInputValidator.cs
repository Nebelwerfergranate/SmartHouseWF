using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SmartHouseWF.Models
{
    public class TimeInputValidator
    {
        private readonly Regex regex = new Regex("^[0-9]{1,2}$");

        public bool GetSeconds(string userInput, out int seconds)
        {
            if (!ValidateString(userInput))
            {
                seconds = 0;
                return false;
            }
            seconds = Int32.Parse(userInput);
            if (seconds < 0 || seconds > 59)
            {
                seconds = 0;
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool GetMinutes(string userInput, out int minutes)
        {
            if (!ValidateString(userInput))
            {
                minutes = 0;
                return false;
            }
            minutes = Int32.Parse(userInput);
            if (minutes < 0 || minutes > 59)
            {
                minutes = 0;
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool GetHours(string userInput, out int hours)
        {
            if (!ValidateString(userInput))
            {
                hours = 0;
                return false;
            }
            hours = Int32.Parse(userInput);
            if (hours < 0 || hours > 23)
            {
                hours = 0;
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateString(string userInput)
        {
            return (regex.Match(userInput)).Success;
        }
    }
}