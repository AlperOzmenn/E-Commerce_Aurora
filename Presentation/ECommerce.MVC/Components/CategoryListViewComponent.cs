using ECommerce.Application.Interfaces;
using ECommerce.Application.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Components
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllAsync();

            var categoryDtos = categories.Select(x => new CategoryDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return View(categoryDtos);
        }
    }
}