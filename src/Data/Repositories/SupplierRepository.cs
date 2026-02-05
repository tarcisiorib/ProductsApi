using Business.Interfaces;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ProductsDbContext dbContext) : base(dbContext) { }

        public async Task<Supplier> GetSupplierAddress(Guid supplierId)
        {
            return await Db.Suppliers.AsNoTracking()
                .Include(a => a.Address)
                .FirstOrDefaultAsync(s => s.Id == supplierId);
        }

        public async Task<Supplier> GetSupplierProductsAddress(Guid supplierId)
        {
            return await Db.Suppliers.AsNoTracking()
                .Include(p => p.Products)
                .Include(a => a.Address)
                .FirstOrDefaultAsync(s => s.Id == supplierId);
        }
    }
}
