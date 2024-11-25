﻿using Microsoft.AspNetCore.Mvc;
using CustomerChurmPrediction.Services;
using MongoDB.Driver;
using CustomerChurmPrediction.Entities.ProductEntity;
using System.Linq;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/product")]
    public class ProductController : ControllerBase
    {
        IUserService _userService;
        IProductService _productService;
        ILogger<ProductController> _logger;

        public ProductController(IUserService userService, IProductService productService, ILogger<ProductController> logger)
        {
            _userService = userService;
            _productService = productService;
            _logger = logger;
        }
        // Получить список сущностей
		// GET: api/product

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var filter = Builders<Product>.Filter.Empty;
                List<Product> productList = await _productService.FindAllAsync(filter, default);

                return Ok( new {productList = productList } );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Получить сущность по id
		// GET: api/product/{productId}

		[HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetByIdAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest();

            try
            {
                Product product = await _productService.FindByIdAsync(productId);
                if (product is null)
                    return NotFound();

                return Ok( new { product = product } );

            }
            catch(Exception ex)
            {
				throw new Exception(ex.Message);
			}
        }
        // Получить список продуктов по id категории
        // GET: api/product/category/{categoryId}

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetByCategoryIdAsync(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                return BadRequest();
            try
            {
                List<Product> productList = await _productService.FindByCategoryIdAsync(categoryId);
                if (productList is null)
                    return NotFound();

                return Ok(new { productList = productList });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Получить список продуктов по id компании
        // GET: api/product/company/{companyId}

        [HttpGet]
        [Route("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyIdAsync(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return BadRequest();
            try
            {
                var productList = await _productService.FindByCompanyIdAsync(companyId, default);

                if (productList is not null)
                    return Ok(new { productList = productList });

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Получить определённое число записей
        // GET: api/product/{pageSize, pageNumber}
        [HttpGet]
        [Route("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> FetchProductsAsync(string pageNumber, string pageSize)
        {
            if (string.IsNullOrEmpty(pageNumber) && string.IsNullOrEmpty(pageSize))
                return BadRequest();
            try
            {
                if(Int32.TryParse(pageNumber, out int number) && Int32.TryParse(pageSize, out int size))
                {
                    List<Product> productList = await _productService.FindAllAsync(default, default);
                    if (productList is not null)
                    {
                        var resultList = productList
                            .Skip(number * size)
                            .Take(size)
                            .ToList();
                        return Ok(new {productList = resultList});
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: /api/product/search/{input}

        [HttpGet]
        [Route("search/{input}")]
        public async Task<IActionResult> GetBySearchStringAsync(string input)
        {
            if(string.IsNullOrEmpty(input))
                return BadRequest();
            try
            {
                var productList = await _productService.FindBySearchStringAsync(input, default);

                if (productList is not null)
                    return Ok(new { productList = productList });
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProductAdd productAdd)
        {
            if(productAdd is null)
                return BadRequest();
			try
			{
                Product product = new Product
                {
                    Name = productAdd.Name,
                    Description = productAdd.Description,
                    CategoryId = productAdd.CategoryId,
                    CompanyId = productAdd.CompanyId,
                    Price = productAdd.Price
                };
                bool isSuccess = await _productService.SaveOrUpdateAsync(product, default);
                if(isSuccess)
                    return Ok(new {product = product} );
                
                // Возвращаю ошибку сервера
                return StatusCode(500);
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
            }
		}
        // Изменить сущность
		// Put: api/product/{productId}

		[HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateAsync(string productId, [FromBody] ProductUpdate productUpdate)
        {
			if (productId is null || productUpdate is null)
				return BadRequest();
			try
			{
                var product = await _productService.FindByIdAsync(productId, default);
                if(product != null)
                {
                    product.Name = productUpdate.Name;
                    var isSuccess = await _productService.SaveOrUpdateAsync(product, default);
                    if(isSuccess)
                        return Ok(new {product = product} );

                    // Возвращаю ошибку сервера
                    return StatusCode(500);
                }
                return NotFound();
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
            }
		}
        // Удалить сущность
        // Delete: api/product/{productId}

        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteAsync(string productId)
        {
			if (string.IsNullOrEmpty(productId))
				return BadRequest();
			try
			{
                var deletedCount = await _productService.DeleteAsync(productId);
                if (deletedCount > 0)
                    return Ok(new { deletedCount = deletedCount });

                return NotFound();
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
            }
		}
    }
}
