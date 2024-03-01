namespace Devenant
{
    public enum PurchaseType
    {
        Purchase
    }

    public class Purchase
    {
        public readonly string name;
        public readonly PurchaseType type;
        public readonly string price;

        public bool purchased;

        public Purchase(string name, PurchaseType type, string price, bool purchased)
        {
            this.name = name;
            this.type = type;
            this.price = price;
            this.purchased = purchased;
        }
    }
}
