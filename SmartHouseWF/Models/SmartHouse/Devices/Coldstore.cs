namespace SmartHouse
{
    public class Coldstore : FridgeModule, IBacklight
    {
        // Fields
        private readonly Lamp backlight;


        // Constructors
        public Coldstore(uint volume, Lamp lamp)
            : base("coldstore", volume)
        {
            backlight = lamp;
            Temperature = 0;
        }


        // Properties
        public override double Temperature
        {
            get { return temperature; }
            set
            {
                if (value < 5 && value > -5)
                {
                    temperature = value;
                }
            }
        }

        public double LampPower
        {
            get { return backlight.Power; }
        }
        public bool IsHighlighted
        {
            get { return backlight.IsOn; }
        }


        // Methods
        public override void TurnOn()
        {
            base.TurnOn();
            if (this.isOpen)
            {
                backlight.TurnOn();
            }
        }
        public override void TurnOff()
        {
            base.TurnOff();
            backlight.TurnOff();
        }
        public override void Open()
        {
            isOpen = true;
            if (this.IsOn)
            {
                backlight.TurnOn();
            }
        }
        public override void Close()
        {
            isOpen = false;
            if (backlight.IsOn)
            {
                backlight.TurnOff();
            }
        }
    }
}
