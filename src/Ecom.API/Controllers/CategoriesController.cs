using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork UOW,IMapper mapper)
        {
            _uOW = UOW;
            _mapper = mapper;
        }

        [HttpGet("get-all-categories")]
        public async Task<ActionResult> Get()
        {
            var allCategories =await  _uOW.CategoryRepository.GetAllAsync();
           
            if (allCategories is not null)
            {
                var res = _mapper.Map<IReadOnlyList<Category>,IReadOnlyList<ListingCategoryDto>>(allCategories);
                //var res = allCategories.Select(x => new ListingCategoryDto
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    Description = x.Description
                //}).ToList();
                return Ok(res);

            }
            return BadRequest("Not Found");
        }
        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var category = await _uOW.CategoryRepository.GetAsync(id);
            if (category == null)
                return BadRequest($"Not Found This id [{id}]");

            //var newCatgoryDto = new ListingCategoryDto
            //{
            //    Id = category.Id,
            //    Name = category.Name,
            //    Description = category.Description
            //};
            return Ok(_mapper.Map<Category,ListingCategoryDto>(category));
        }
        [HttpPost("add-new-category")]
        public async Task<ActionResult> Post(CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var newCategory = new Category
                    //{
                    //    Name = categoryDto.Name,
                    //    Description = categoryDto.Description
                    //};
                    var res = _mapper.Map<Category>(categoryDto);
                    await _uOW.CategoryRepository.AddAsync(res);
                    return Ok(categoryDto);
                }
                return BadRequest(categoryDto);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPut("update-exiting-category-by-id")]
        public async Task<ActionResult> Put(UpdateCategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exitingCategory = await _uOW.CategoryRepository.GetAsync(categoryDto.Id);
                    if (exitingCategory is not null)
                    {
                        //Updating
                        //exitingCategory.Name = categoryDto.Name;
                        //exitingCategory.Description = categoryDto.Description;
                        _mapper.Map(categoryDto, exitingCategory);
                       await _uOW.CategoryRepository.UpdateAsync(categoryDto.Id, exitingCategory);
                        return Ok(categoryDto);
                    }
                }
               
                return NotFound($"Category Not Found , Id [{categoryDto.Id}] Incorrect");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
           
        }
        [HttpDelete("delete-category-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var exitingCategory = await _uOW.CategoryRepository.GetAsync(id);
                if(exitingCategory is not null)
                {
                   await _uOW.CategoryRepository.DeleteAsync(id);
                    return Ok($"This category [{exitingCategory.Name}] is deleted Successfully ");
                }
                return NotFound($"Category Not Found , Id [{id}] Incorrect");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
