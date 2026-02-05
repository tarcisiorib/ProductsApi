using Business.Interfaces;
using Business.Models;
using Business.Models.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;

        public SupplierService(ISupplierRepository supplierRepository,
                               IAddressRepository addressRepository,
                               INotifier notifier) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
        }

        public async Task<bool> Add(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidator(), supplier)
                || !ExecuteValidation(new AddressValidator(), supplier.Address)) return false;

            if (_supplierRepository.Search(s => s.Document == supplier.Document).Result.Any())
            {
                Notify("Document already registrated");
                return false;
            }

            await _supplierRepository.Add(supplier);
            return true;
        }

        public async Task<bool> Update(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidator(), supplier)) return false;
            if (_supplierRepository.Search(s => s.Document == supplier.Document && s.Id != supplier.Id).Result.Any())
            {
                Notify("Document already registrated");
                return false;
            }

            await _supplierRepository.Update(supplier);
            return true;
        }

        public async Task<bool> Remove(Guid id)
        {
            if (_supplierRepository.GetSupplierProductsAddress(id).Result.Products.Any())
            {
                Notify("Supplier with products registrated");
                return false;
            }

            var address = await _addressRepository.GetAddressBySupplier(id);

            if (address != null)
                await _addressRepository.Remove(address.Id);

            await _supplierRepository.Remove(id);

            return true;
        }

        public async Task UpdateAddress(Address address)
        {
            if (!ExecuteValidation(new AddressValidator(), address)) return;
            await _addressRepository.Update(address);
        }

        public void Dispose()
        {
            _supplierRepository?.Dispose();
            _addressRepository?.Dispose();
        }
    }
}
