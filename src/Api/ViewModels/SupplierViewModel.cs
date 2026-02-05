using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels
{
    public class SupplierViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Document { get; set; }
        [Range(1,2)]
        public int Type { get; set; }
        public AddressViewModel Address { get; set; }
        public bool Active { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
