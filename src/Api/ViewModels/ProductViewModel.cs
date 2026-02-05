using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageUpload { get; set; }
        public string Image { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ScaffoldColumn(false)]
        public DateTime Registration { get; set; }
        public bool Active { get; set; }
        [ScaffoldColumn(false)]
        public string SupplierName { get; set; }
    }
}
