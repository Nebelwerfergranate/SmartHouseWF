using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseWF.Models.DeviceManager
{
    public class SessionDeviceManager
    {
        // Fields
        private SortedDictionary<uint, Device> myDevices;
        private readonly HttpContext context = HttpContext.Current;
        private uint newDeviceID;


        // Constructors
        public SessionDeviceManager(bool doInit = false)
        {
            if (doInit)
            {
                Init();
            }
            else
            {
                myDevices = (SortedDictionary<uint, Device>)context.Session["devices"];
                UInt32.TryParse(context.Session["newDeviceID"].ToString(), out newDeviceID);
            }
        }

        // Methods
        public SortedDictionary<uint, Device> GetDevices()
        {
            return myDevices;
        }

        public string AddClock(string name)
        {
            Clock clock = new Clock(name);
            AddDevice(clock);
            string message = "Clock " + name + " has been successfully added.";
            return message;
        }

        public string AddOven(string name)
        {
            Oven oven = new Oven(name, 47, new Lamp(25));
            oven.OperationDone += (sender) =>
            {
                //throw new ApplicationException("Печка отработала!!!");
            };
            AddDevice(oven);
            string message = "Oven " + name + "  has been successfully added.";
            return message;
        }

        public string AddMicrowave(string name)
        {
            Microwave microwave = new Microwave(name, 25, new Lamp(25));
            microwave.OperationDone += (sender) =>
            {
                //throw new ApplicationException("Микроволновка отработала!!!");
            };
            AddDevice(microwave);
            string message = "Microwave " + name + "  has been successfully added.";
            return message;
        }

        public string AddFridge(string name)
        {
            Fridge fridge = new Fridge(name, new Coldstore(100, new Lamp(15)), new Refrigeratory(50));
            AddDevice(fridge);
            string message = "Fridge " + name + "  has been successfully added.";
            return message;
        }

        public void RemoveById(uint id)
        {
            if (myDevices.ContainsKey(id))
            {
                myDevices.Remove(id);
                context.Session["devices"] = GetDevices();
            }
        }

        public Device GetDeviceById(uint id)
        {
            Device deviceFound = null;
            if (myDevices.ContainsKey(id))
            {
                deviceFound = myDevices[id];
            }
            return deviceFound;
        }

        private void Init()
        {
            // Для тестирования начальные девайсы добавлять сюда.
            myDevices = new SortedDictionary<uint, Device>();
            newDeviceID = 0;
            AddClock("myClock");
            context.Session["devices"] = GetDevices();
            context.Session["newDeviceID"] = newDeviceID;
        }
        private void AddDevice(Device device)
        {
            myDevices.Add(newDeviceID, device);
            newDeviceID++;
            context.Session["devices"] = GetDevices();
            context.Session["newDeviceID"] = newDeviceID;
        }
    }
}
