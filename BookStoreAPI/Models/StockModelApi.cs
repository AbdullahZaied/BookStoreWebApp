namespace BookStoreAPI.Models
{
    public class StockModelApi
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int StockAmount { get; set; }
        public int IncStockBy { get; set; }
        public int DecStockBy { get; set; }
        public DateTime StockedAt { get; set; }
        public string StockedBy { get; set; }

    }
}
