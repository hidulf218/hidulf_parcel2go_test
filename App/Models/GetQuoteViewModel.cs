using Core.Model;
using Core.Model.Result;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class GetQuoteViewModel
    {
        public Token Token { get; set; }

        [Required]
        [Display(Name = "Weight")]
        [DataType(DataType.Currency)]
        public decimal? InputWeight { get; set; }
        [Required]
        [Display(Name = "Length")]
        public decimal InputLength { get; set; }
        [Required]
        [Display(Name = "Width")]
        public decimal InputWidth { get; set; }
        [Required]
        [Display(Name = "Height")]
        public decimal InputHeght { get; set; }
        [Required]
        [Display(Name = "Collection Country")]
        public string InputCollectionCountry { get; set; }
        [Required]
        [Display(Name = "Delivery Country")]
        public string InputDeliveryCountry { get; set; }
        [Required]
        [Display(Name = "Collection Type")]
        public string InputCollectionType { get; set; }
        [Required]
        [Display(Name = "Delivery Type")]
        public string InputDeliveryType { get; set; }

        public Quote[] Quotes { get; set; }

        public string ErrorMessage { get; set; }
        public string SortCol { get; set; }
    }
}
