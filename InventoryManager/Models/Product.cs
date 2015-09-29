namespace InventoryManager
{
    public class Product
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _supplier;

        public string Supplier
        {
            get
            {
                return _supplier;
            }
            set
            {
                _supplier = value;
            }
        }

        private double _price;
        public double Price {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            } 
        }

        private double _cost;

        public double Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
            }
        }

        public int Profit
        {
            get
            {
                return (int) (_price - _cost);
            }
        }

        public int Margin
        {
            get
            {
                return (int) (((_price-_cost)/_price)*100);
            }
        }
    }
}
