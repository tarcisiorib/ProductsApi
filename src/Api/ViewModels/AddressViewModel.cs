using System;
using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels
{
    public class AddressViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        public Guid SupplierId { get; set; }
    }
}
