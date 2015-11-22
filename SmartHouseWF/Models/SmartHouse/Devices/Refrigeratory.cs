namespace SmartHouse
{
    public class Refrigeratory : FridgeModule
    {
        // Fields
        private readonly double minTemperature = -6;
        private readonly double maxTemperature = -30;

        // Constructors
        public Refrigeratory(uint volume)
            : base("refrigeratory", volume)
        {
            Temperature = -10;
        }


        // Properties
        public override double Temperature
        {
            get { return temperature; }
            set
            {
                if (value <= MinTemperature && value >= MaxTemperature)
                {
                    temperature = value;
                }
            }
        }
        public override double MinTemperature
        {
            get { return minTemperature; }
        }

        public override double MaxTemperature
        {
            get { return maxTemperature; }
        }
    }
}
