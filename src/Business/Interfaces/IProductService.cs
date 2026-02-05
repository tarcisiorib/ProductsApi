using Business.Models;
using System;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task Add(Product supplier);
        Task Update(Product supplier);
        Task Remove(Guid id);
    }
}
