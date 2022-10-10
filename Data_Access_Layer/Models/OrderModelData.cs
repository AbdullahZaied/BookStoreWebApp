namespace Data.Access.Layer.Models
{
    public class OrderModelData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public int OrderAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
