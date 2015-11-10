using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseWF.Models.DeviceManager
{
    public class DeviceManager
    {
        // fields
        private List<Device> myDevices;


        // Constructors
        public DeviceManager(Device[] devices)
        {
            myDevices = devices.ToList();
        }

        //methods
        public Device[] GetDevices()
        {
            return myDevices.ToArray();
        }

        public string AddClock(string name)
        {
            string message = "";
            if (GetDeviceByName(name) == null)
            {
                myDevices.Add(new Clock(name));
                message = "Часы с именем " + name + " добавлены.";
            }
            else
            {
                message = "Устройство с таким именем уже существует!";
            }
            return message;
        }

        public void RemoveByName(string name)
        {
            Device deviceToRemove = GetDeviceByName(name);
            if (deviceToRemove != null)
            {
                myDevices.Remove(deviceToRemove);
            }
        }

        public Device GetDeviceByName(string name)
        {
            Device deviceFound = null;
            foreach (Device device in myDevices)
            {
                if (device.Name == name)
                {
                    deviceFound = device;
                }
            }
            return deviceFound;
        }
    }
}