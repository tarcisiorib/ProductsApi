using Business.Models;
using System;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<Supplier> GetSupplierAddress(Guid supplierId);
        Task<Supplier> GetSupplierProductsAddress(Guid supplierId);
    }
}
