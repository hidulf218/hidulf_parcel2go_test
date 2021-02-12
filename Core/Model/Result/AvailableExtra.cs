namespace Core.Model.Result
{
    public class AvailableExtra
    {
        public string Type { get; set; }
        public decimal Price { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public AvailableExtraDetails Details { get; set; }
    }
}
