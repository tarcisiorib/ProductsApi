using System;

namespace Business.Models
{
    public class Address : Entity
    {
        public Guid SupplierId { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public Supplier Supplier { get; set; }
    }
}
