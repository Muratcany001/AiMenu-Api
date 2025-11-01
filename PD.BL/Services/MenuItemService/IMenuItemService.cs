using Common.ViewModels;
using Dtos.MenuItemDto;
using Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.MenuItemService
{
    public interface IMenuItemService
    {
        Task<ResultViewModel<MenuItemDto>> AddMenuItem(AddMenuItemDto addMenuItemDto);
        Task<ResultViewModel<MenuItemDto>> GetMenuItemById(int id);
        Task<ResultViewModel<List<MenuItemDto>>> GetAllMenuItems();
        Task<ResultViewModel<MenuItemDto>> UpdateMenuItem(int id, UpdateMenuItemDto updateMenuItemDto);
        Task<ResultViewModel<object>> DeleteMenuItem(int id);
        Task<ResultViewModel<List<MenuItemDto>>> GetMenuItemsByCategory(string category);
        Task<ResultViewModel<List<MenuItemDto>>> SearchMenuItems(string searchTerm);
        Task<ResultViewModel<List<MenuItemDto>>> GetMenuItemsByPriceRange(decimal minPrice, decimal maxPrice);
        Task<ResultViewModel<List<MenuItemDto>>> GetMenuItemByIngeredients(string ingredients);
    }
}
