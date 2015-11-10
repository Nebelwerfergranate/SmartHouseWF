using System;

namespace SmartHouse
{
    public static class MicrowaveFactory
    {
        public static Microwave GetWhirpool(string name)
        {
            Microwave oven = new Microwave(name, 20, new Lamp(25));
            oven.OperationDone += source =>
            {
                Console.Beep(3000, 500);
                Console.WriteLine("Ваша тарелка нагрета");
            };
            return oven;
        }

        public static Microwave GetPanasonic(string name)
        {
            Microwave oven = new Microwave(name, 19, new Lamp(20));
            oven.OperationDone += source =>
            {
                Console.Beep(3000, 500);
                Console.WriteLine("Ваша тарелка нагрета");
            };
            return oven;
        }

        public static Microwave GetLg(string name)
        {
            Microwave oven = new Microwave(name, 23, new Lamp(25));
            oven.OperationDone += source =>
            {
                Console.Beep(3000, 500);
                Console.WriteLine("Ваша тарелка нагрета");
            };
            return oven;
        }
    }
}
