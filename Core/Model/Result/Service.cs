namespace Core.Model.Result
{
    public class Service
    {
        public string DropOffProviderCode { get; set; }
        public string CourierName { get; set; }
        public string CourierSlug { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string CollectionType { get; set; }
        public string DeliveryType { get; set; }
        public Link Links { get; set; }
        public decimal? MaxHeight { get; set; }
        public decimal? MaxWidth { get; set; }
        public decimal? MaxLength { get; set; }
        public decimal? MaxWeight { get; set; }
        public decimal? MaxCover { get; set; }
        public bool IsPrinterRequired { get; set; }
        public string ShortDescription { get; set; }
        public string Overview { get; set; }
        public string Features { get; set; }
        public bool RequiresDimensions { get; set; }
        public string Classification { get; set; }
    }
}
