namespace SmartHouse
{
    public class Fridge : Device
    {
        // Fields
        private readonly Coldstore coldstore;
        private readonly Refrigeratory refrigeratory;


        // Constructors
        public Fridge(string name, Coldstore coldstore, Refrigeratory refrigeratory) : base(name)
        {
            this.coldstore = coldstore;
            this.refrigeratory = refrigeratory;
        }


        // Properties
        public bool ColdstoreIsOpen
        {
            get { return coldstore.IsOpen; }
        }
        public double ColdstoreTemperature
        {
            get { return coldstore.Temperature; }
            set { coldstore.Temperature = value; }
        }

        public double ColdstoreMinTemperature
        {
            get { return coldstore.MinTemperature; }
        }

        public double ColdstoreMaxTemperature
        {
            get { return coldstore.MaxTemperature; }
        }
        public bool ColdstoreIsHighlighted
        {
            get { return coldstore.IsHighlighted; }
        }
        public double ColdstoreLampPower
        {
            get { return coldstore.LampPower; }
        }
        public double ColdstoreVolume
        {
            get { return coldstore.Volume; }
        }

        public bool RefrigeratoryIsOpen
        {
            get { return refrigeratory.IsOpen; }
        }
        public double RefrigeratoryTemperature
        {
            get { return refrigeratory.Temperature; }
            set { refrigeratory.Temperature = value; }
        }

        public double RefrigeratoryMinTemperature
        {
            get { return refrigeratory.MinTemperature; }
        }

        public double RefrigeratoryMaxTemperature
        {
            get { return refrigeratory.MaxTemperature; }
        }
        public double RefrigeratoryVolume
        {
            get { return refrigeratory.Volume; }
        }


        // Methods
        public override void TurnOn()
        {
            base.TurnOn();
            coldstore.TurnOn();
            refrigeratory.TurnOn();
        }
        public override void TurnOff()
        {
            base.TurnOff();
            coldstore.TurnOff();
            refrigeratory.TurnOff();
        }

        public void OpenColdstore()
        {
            coldstore.Open();
        }
        public void CloseColdstore()
        {
            coldstore.Close();
        }
        public void OpenRefrigeratory()
        {
            refrigeratory.Open();
        }
        public void CloseRefrigeratory()
        {
            refrigeratory.Close();
        }
    }
}
