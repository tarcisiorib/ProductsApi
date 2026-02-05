using Api.Extensions;
using Api.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/suppliers")]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository,
                                   ISupplierService supplierService,
                                   IAddressRepository addressRepository,
                                   INotifier notifier,
                                   IMapper mapper,
                                   IUser user) : base(notifier, user)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<SupplierViewModel>> Get()
            => _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
        

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Get(Guid id)
        {
            var supplierViewModel = await GetSupplierProductsAddress(id);

            if (supplierViewModel == null)
                return NotFound();

            return supplierViewModel;
        }

        [ClaimsAuthorize("Supplier", "C")]
        [HttpPost()]
        public async Task<ActionResult<SupplierViewModel>> Post(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) 
                return CustomResponse(ModelState);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            await _supplierService.Add(supplier);

            return CustomResponse(_mapper.Map<SupplierViewModel>(supplier));
        }

        [ClaimsAuthorize("Supplier", "U")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Put(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id)
            {
                NotifyError("The provided id does not match the supplier id.");
                return CustomResponse(supplierViewModel);
            }

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.Update(supplier);

            return CustomResponse(_mapper.Map<SupplierViewModel>(supplier));
        }

        [ClaimsAuthorize("Supplier", "D")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null)
                return NotFound();

            await _supplierService.Remove(id);

            return CustomResponse();
        }

        [HttpGet("address/{id:guid}")]
        public async Task<AddressViewModel> GetAddress(Guid id)
        {
            return _mapper.Map<AddressViewModel>(await _addressRepository.GetById(id));
        }

        [ClaimsAuthorize("Supplier", "U")]
        [HttpPut("address/{id:guid}")]
        public async Task<IActionResult> PutAddress(Guid id, AddressViewModel addressViewModel)
        {
            if (id != addressViewModel.Id)
            {
                NotifyError("The provided id does not match the address id.");
                return CustomResponse(addressViewModel);
            }
                
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var address = _mapper.Map<Address>(addressViewModel);

            await _supplierService.UpdateAddress(address);

            return CustomResponse(_mapper.Map<AddressViewModel>(address));
        }

        private async Task<SupplierViewModel> GetSupplierProductsAddress(Guid id)
            => _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierProductsAddress(id));

        private async Task<SupplierViewModel> GetSupplierAddress(Guid id)
            => _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddress(id));
    }
}
