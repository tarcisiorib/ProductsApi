using Api.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,
                                  IProductService productService,
                                  IMapper mapper,
                                  INotifier notifier,
                                  IUser user) : base(notifier, user)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> Get()
            => _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsSuppliers());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>>Get(Guid id)
        {
            var productViewModel = await GetProductSupplier(id);

            if (productViewModel == null)
                return NotFound();
            
            productViewModel.ImageUpload = await GetImage(productViewModel.Image);

            return productViewModel;
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> Post(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var imageName = Guid.NewGuid() + "_" + productViewModel.Image;

            if (!UploadImage(productViewModel.ImageUpload, imageName))
                return CustomResponse(productViewModel);

            productViewModel.Image = imageName;
            await _productService.Add(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Put(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                NotifyError("The provided id does not match the product id.");
                return CustomResponse(productViewModel);
            }

            var productStored = await GetProductSupplier(id);
            productViewModel.Image = productStored.Image;

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (productViewModel.ImageUpload != null)
            {
                var imageName = Guid.NewGuid() + "_" + productViewModel.Image;
                if (!UploadImage(productViewModel.ImageUpload, imageName))
                    return CustomResponse(ModelState);

                productStored.Image = imageName;
            }

            productStored.Name = productViewModel.Name;
            productStored.Description = productViewModel.Description;
            productStored.Price = productViewModel.Price;
            productStored.Active = productViewModel.Active;

            await _productService.Update(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Delete(Guid id)
        {
            var productViewModel = await GetProductSupplier(id);

            if (productViewModel == null)
                return NotFound();

            await _productService.Remove(id);
            
            return CustomResponse(productViewModel);
        }

        private async Task<ProductViewModel> GetProductSupplier(Guid id)
            => _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplier(id));

        private bool UploadImage(string file, string name)
        {
            if (string.IsNullOrEmpty(file))
            {
                NotifyError("Provide an image for this product!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(file);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", name);

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("A file with this name already exists!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        private async Task<string> GetImage(string name)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", name);

            if (!System.IO.File.Exists(filePath))
                return null;

            var imageDataByteArray = await System.IO.File.ReadAllBytesAsync(filePath);

            return Convert.ToBase64String(imageDataByteArray);
        }
    }
}
