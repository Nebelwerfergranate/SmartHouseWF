using System;

namespace SmartHouse
{
    public static class OvenFactory
    {
        public static Oven GetSiemense(string name)
        {
            Oven oven = new Oven(name, 67, new Lamp(25));
            oven.OperationDone += source =>
            {
                Console.Beep(2000, 1000);
                Console.WriteLine(source.Name + ": Выполнение операции закончено.");
            };
            return oven;
        }

        public static Oven GetElectrolux(string name)
        {
            Oven oven = new Oven(name, 74, new Lamp(25));
            oven.OperationDone += source =>
            {
                Console.Beep(2000, 1000);
                Console.WriteLine(source.Name + ": Выполнение операции закончено.");
            };
            return oven;
        }

        public static Oven GetPyramida(string name)
        {
            Oven oven = new Oven(name, 56, new Lamp(15));
            oven.OperationDone += source =>
            {
                Console.Beep(2000, 1000);
                Console.WriteLine(source.Name + ": Выполнение операции закончено.");
            };
            return oven;
        }
    }
}
