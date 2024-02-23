namespace Devenant
{
    public enum PurchaseType
    {
        Purchase
    }

    public class Purchase
    {
        public readonly string id;
        public readonly PurchaseType type;
        public readonly string price;

        public bool purchased;

        public Purchase(string id, PurchaseType type, string price, bool purchased)
        {
            this.id = id;
            this.type = type;
            this.price = price;
            this.purchased = purchased;
        }
    }
}
