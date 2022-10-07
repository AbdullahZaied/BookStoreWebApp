namespace Data.Access.Layer.Models
{
    public class StockModelData
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int StockAmount { get; set; }
        public DateTime StockedAt { get; set; }
        public string StockedBy { get; set; }
    }
}
