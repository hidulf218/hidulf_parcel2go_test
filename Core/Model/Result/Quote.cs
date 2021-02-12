using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Model.Result
{
    public class Quote
    {
        public AvailableExtra[] AvailableExtras { get; set; }
        public decimal Discount { get; set; }
        public Service Service { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPriceExVat { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalVat { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; }
        public decimal VatRate { get; set; }
        public DateTime Collection { get; set; }
        public DateTime CutOff { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public decimal IncludedCover { get; set; }
        public bool RequiresCommercialInvoice { get; set; }
    }
}
