using Common.ViewModels;
using Dtos.MenuItemDto;
using Microsoft.AspNetCore.Mvc;
using PD.BL.Services.MenuItemService;

namespace PD.UI.Controllers
{
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpPost("api/menuItems/addMenuItem")]
        public async Task<IActionResult> addMenuItem(AddMenuItemDto addMenuItemDto)
        {
            var result = await _menuItemService.AddMenuItem(addMenuItemDto);
            return Ok(result);
        }

        [HttpDelete("api/menuItems/deleteMenuItem/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var result = await _menuItemService.DeleteMenuItem(id);
            return Ok(result);
        }

        [HttpGet("api/menuItems/getAllMenuItems")]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var result = await _menuItemService.GetAllMenuItems();
            return Ok(result);
        }

        [HttpGet("api/menuItems/getMenuItemById/{id}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var result = await _menuItemService.GetMenuItemById(id);
            return Ok(result);
        }

        [HttpPut("api/menuItems/updateMenuItem/{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, UpdateMenuItemDto updateMenuItemDto)
        {
            var result = await _menuItemService.UpdateMenuItem(id, updateMenuItemDto);
            return Ok(result);
        }

        [HttpGet("api/menuItems/getMenuItemsByCategory/{category}")]
        public async Task<IActionResult> GetMenuItemsByCategory(string category)
        {
            var result = await _menuItemService.GetMenuItemsByCategory(category);
            return Ok(result);
        }

        [HttpGet("api/menuItems/searchMenuItems")]
        public async Task<IActionResult> SearchMenuItems(string searchTerm)
        {
            var result = await _menuItemService.SearchMenuItems(searchTerm);
            return Ok(result);
        }

        [HttpGet("api/menuItems/getMenuItemsByPriceRange")]
        public async Task<IActionResult> GetMenuItemsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var result = await _menuItemService.GetMenuItemsByPriceRange(minPrice, maxPrice);
            return Ok(result);
        }

        [HttpGet("api/menuItems/getMenuItemByIngeredients")]
        public async Task<IActionResult> GetMenuItemByIngeredients(string ingredients)
        {
            var result = await _menuItemService.GetMenuItemByIngeredients(ingredients);
            return Ok(result);
        }
    }
}