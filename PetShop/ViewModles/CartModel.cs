namespace PetShop.ViewModles
{
    public class CartModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Price;

    }
    public class CartSummaryModel
    {
        public List<CartModel> Items { get; set; }
        public decimal TotalValue { get; set; }
        public int TotalQuantity { get; set; }
    }
}
