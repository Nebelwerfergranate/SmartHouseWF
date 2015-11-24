using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse
{
    public static class Informer
    {
        public static string StateToString(Device device)
        {
            string info = "";
            info += GetBaseInfo(device);
            info += GetOpenableInfo(device);
            info += GetBacklightInfo(device);
            info += GetTemperatureInfo(device);
            info += GetClockInfo(device);
            info += GetTimerInfo(device);
            info += GetIvolumeInfo(device);
            info += GetFridgeInfo(device);
            return info;
        }

        private static string GetBaseInfo(Device device)
        {
            string info = "";

            info += "Имя устройства: " + device.Name + "\n";
            if (device.IsOn)
            {
                info += "Cостояние: включен\n";
            }
            else
            {
                info += "Состояние: выключен\n";
            }
            return info;
        }

        private static string GetBacklightInfo(Device device)
        {
            string info = "";
            if (device is IBacklight)
            {

                if (((IBacklight)device).IsHighlighted)
                {
                    info += "Подсветка: включена\n";
                }
                else
                {
                    info += "Подсветка: выключена\n";
                }
                info += "Мощность лампочки: " + ((IBacklight)device).LampPower + " Вт\n";
            }
            return info;
        }

        private static string GetOpenableInfo(Device device)
        {
            string info = "";
            if (device is IOpenable)
            {
                if (((IOpenable)device).IsOpen)
                {
                    info += "Дверца: открыта\n";
                }
                else
                {
                    info += "Дверца: закрыта\n";
                }
            }
            return info;
        }

        private static string GetTemperatureInfo(Device device)
        {
            string info = "";
            if (device is ITemperature)
            {
                info += "Установка термостата: " + ((ITemperature)device).Temperature + " градусов\n";
            }
            return info;
        }

        private static string GetClockInfo(Device device)
        {
            string info = "";
            if (device is IClock)
            {
                info += "Текущее время: " + ((IClock)device).CurrentTime.ToLongTimeString() + "\n";
            }
            return info;
        }

        private static string GetTimerInfo(Device device)
        {
            string info = "";
            if (device is ITimer)
            {
                info += "Статус: ";
                if (((ITimer) device).IsRunning)
                {
                    info += "Выполняет задачу\n";
                }
                else
                {
                    info += "Задач нет\n";
                }
            }
            return info;
        }

        private static string GetIvolumeInfo(Device device)
        {
            string info = "";
            if (device is IVolume)
            {
                info += "Внутренний объем: " + ((IVolume) device).Volume + " л\n";
            }
            return info;
        }

        private static string GetFridgeInfo(Device device)
        {
            string info = "";
            Fridge fridge = device as Fridge;
            if (fridge != null)
            {
                info += "Объём морозильной камеры: " + fridge.FreezeryVolume + " л\n"; 
                info += "Дверца морозильной камеры: ";
                if (fridge.FreezerIsOpen)
                {
                    info += "Открыта\n";
                }
                else
                {
                    info += "Закрыта\n";
                }
                info += "Температура в морозильной камере: " + fridge.FreezerTemperature + " градусов\n";

                info += "Дверца холодильника: ";
                if (fridge.ColdstoreIsOpen)
                {
                    info += "Открыта\n";
                }
                else
                {
                    info += "Закрыта\n";
                }
                info += "Температура в холодильнике: " + fridge.ColdstoreTemperature + " градусов\n";

                info += "Объём холодильника (без морозильной камеры): " + fridge.ColdstoreVolume + " л\n"; 
                info += "Подсветка холодильника: ";
                if (fridge.ColdstoreIsHighlighted)
                {
                    info += "Включена\n";
                }
                else
                {
                    info += "Выключена\n";
                }
                info += "Мощность ламочки холодильника: " + fridge.ColdstoreLampPower + " Вт\n";
            }
            return info;
        }
    }
}
