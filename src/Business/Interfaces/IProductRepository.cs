using Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductSupplier(Guid productId);
        Task<IEnumerable<Product>> GetProductsSuppliers();
        Task<IEnumerable<Product>> GetProductsBySupplier(Guid supplierId);
    }
}
