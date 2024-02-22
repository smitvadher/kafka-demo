namespace OrderApi.Models
{
    public record ProductAvailability
    {
        public int ProductId { get; set; }

        public int AvailableQuantity { get; set; }
    }
}
