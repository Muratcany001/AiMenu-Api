using Common.ViewModels;
using Dtos.MenuItemDto;
using Microsoft.AspNetCore.Mvc;
using PD.BL.Services.MenuItemService;
using PD.BL.Services.RedisCacheService;
using System.Diagnostics;

namespace PD.UI.Controllers
{
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IRedisCacheService _cache;
        private readonly ILogger<MenuItemController> _logger;
        public MenuItemController(IMenuItemService menuItemService, IRedisCacheService cache, ILogger<MenuItemController> logger)
        {
            _menuItemService = menuItemService;
            _cache = cache;
            _logger = logger;
        }

        [HttpPost("api/menuItems/addMenuItem")]
        public async Task<IActionResult> addMenuItem(AddMenuItemDto addMenuItemDto)
        {

            var result = await _menuItemService.AddMenuItem(addMenuItemDto);
            _cache.RemoveData("allMenuItems");
            return Ok(result);
        }

        [HttpDelete("api/menuItems/deleteMenuItem/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var result = await _menuItemService.DeleteMenuItem(id);
            //item silinecegi zaman cache deki tum menu itemlari kaldir
            _cache.RemoveData("allMenuItems");
            return Ok(result);
        }

        [HttpGet("api/menuItems/getAllMenuItems")]
        public async Task<IActionResult> GetAllMenuItems()
        {
            // Istenilen veriyi cache de ara
            var cachedData = _cache.GetData<List<MenuItemDto>>("allMenuItems");
            // Eger varsa dondur
            if (cachedData != null) {
                var response = ResultViewModel<List<MenuItemDto>>.Success(cachedData, "Retrieved from cache", 200);
                return Ok(response);
            }
            //Eger cache icerisinde yoksa cache den al
            var serviceResult = await _menuItemService.GetAllMenuItems();
            _cache.SetData("allMenuItems", serviceResult.Data);
            return Ok(serviceResult);
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
            _cache.RemoveData("allMenuItems");
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
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("User try to connect gemini service");
            var result = await _menuItemService.SearchMenuItems(searchTerm);
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 15000) {
                _logger.LogWarning("User cannot to connect gemini service | {stopwatch}", stopwatch);
            }
            if(result == null)
            {
                _logger.LogWarning("User cannot to connect gemini service");
            }
            _logger.LogInformation("Gemini answer | {searchTerm}", searchTerm);
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