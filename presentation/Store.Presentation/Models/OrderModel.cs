namespace Store.Presentation.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public OrderItemModel[] Items { get; set; } = new OrderItemModel[0];

        public int TotalCount { get; set; }

        public decimal TotalPrice { get; set; }
<<<<<<< HEAD

        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
=======
>>>>>>> 1994a45 (Revert "Merge branch 'refactoring/orders'")
    }
}
