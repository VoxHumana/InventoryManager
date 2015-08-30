namespace InventoryManager
{
    public class Product
    {
        public string Name { get; set; }
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
        private double _price;

        public int Margin
        {
            get
            {
                return (int) (((_price-_cost)/_price)*100);
            }
        }
    }
}
