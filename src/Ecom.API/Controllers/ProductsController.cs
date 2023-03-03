using AutoMapper;
using Ecom.API.Errors;
using Ecom.API.Helper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;

        public ProductsController(IUnitOfWork UOW, IMapper mapper, IWebHostEnvironment webHost)
        {
            _uOW = UOW;
            _mapper = mapper;
            _webHost = webHost;
        }
        [HttpGet("get-all-products")]
        public async Task<ActionResult> Get([FromQuery]ProductParams productParams)
        {
            //var src = await _uOW.ProductRepository.GetAllAsync(x => x.Category);
            var src = await _uOW.ProductRepository.GetAllAsync(productParams);
            var result = _mapper.Map<IReadOnlyList<ProductDto>>(src.ProductDtos);

            return Ok(new Pagination<ProductDto>(productParams.PageNumber,productParams.PageSize,src.TotalItems,result));
        }
        [HttpGet("get-product-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            var src = await _uOW.ProductRepository.GetByIdAsync(id, x => x.Category);
            if (src is null)
                return NotFound(new BaseCommonResponse(404));
            var result = _mapper.Map<ProductDto>(src);
            return Ok(result);
        }
        [HttpPost("add-new-product")]
        public async Task<ActionResult> Post([FromForm] CreateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var res = await _uOW.ProductRepository.AddAsync(productDto);
                    return res ? Ok(productDto) : BadRequest(res);
                }
                return BadRequest(productDto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPut("update-exiting-product/{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] UpdateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uOW.ProductRepository.UpdateAsync(id, productDto);
                    return res ? Ok(productDto) : BadRequest();
                }
                return BadRequest(productDto);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-exiting-product/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uOW.ProductRepository.DeleteAsyncWithPicture(id);
                    return res ? Ok(res) : BadRequest(res);
                }
                return NotFound($" This is {id} Not Found");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
