namespace Core.Model.Parameter
{
    public class QuoteParameter
    {
        public Address CollectionAddress { get; set; }
        public Address DeliveryAddress { get; set; }
        public Parcel[] Parcels { get; set; }
    }
}
