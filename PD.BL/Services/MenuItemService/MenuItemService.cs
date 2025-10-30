using AutoMapper;
using Common.ViewModels;
using Dtos.MenuItemDto;
using Dtos.UserDtos;
using FluentValidation;
using PD.DAL.Entitites.AppEntitites;
using PD.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.MenuItemService
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IBaseRepository<MenuItem> _baseRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<MenuItem> _createMenuItemValidator;
        public MenuItemService(IBaseRepository<MenuItem> menuItemRepository, IMapper mapper, IValidator<MenuItem> _validator)
        {
            _baseRepository = menuItemRepository;
            _mapper = mapper;
            _createMenuItemValidator = _validator;
        }

        public async Task<ResultViewModel<MenuItemDto>> AddMenuItem(AddMenuItemDto addMenuItemDto)
        {
            var existedItem = await _baseRepository.GetAsync(x => x.Name.Equals(addMenuItemDto.Name));
            if (existedItem != null)
            {
                return ResultViewModel<MenuItemDto>.Failure("this menu already exists",null, 400);
            }
            var menuItemEntity =  _mapper.Map<MenuItem>(addMenuItemDto);
            await _baseRepository.AddAsync(menuItemEntity);
            var data = _mapper.Map<MenuItemDto>(menuItemEntity);
            return ResultViewModel<MenuItemDto>.Success(data, "menu item added successfully", 201);
        }

        public async Task<ResultViewModel<object>> DeleteMenuItem(int id)
        {
            var existedItem = await _baseRepository.GetByIdAsync(id);
            if (existedItem == null)
            {
                return ResultViewModel<object>.NotFound("Menu item not found",404);
            }
            await _baseRepository.DeleteAsync(existedItem);
            return ResultViewModel<object>.Success(null, "Menu item deleted successfully", 200);

        }

        public async Task<ResultViewModel<List<MenuItemDto>>> GetAllMenuItems()
        {
            var existedItems = await  _baseRepository.GetAsync(asNoTracking: false);
            if (existedItems == null)
            {
                return ResultViewModel<List<MenuItemDto>>.NotFound("No menu items found",404);
            }
            var list = _mapper.Map<List<MenuItemDto>>(existedItems);
            return ResultViewModel<List<MenuItemDto>>.Success(list, "Menu items retrieved successfully", 200);
        }

        public async Task<ResultViewModel<MenuItemDto>> GetMenuItemById(int id)
        {
            var existedItems = await _baseRepository.GetByIdAsync(id);
            if (existedItems == null)
            {
                return ResultViewModel<MenuItemDto>.NotFound("Menu item not found",404);
            }
            var data = _mapper.Map<MenuItemDto>(existedItems);
            return ResultViewModel<MenuItemDto>.Success(data, "Menu item retrieved successfully", 200);
        }

        public async Task<ResultViewModel<MenuItemDto>> GetMenuItemByIngeredients(string ingredients)
        {
            var existedItems = await _baseRepository.GetAsync(x => x.Ingeredents.Equals(ingredients));
            if (existedItems == null)
            {
                return ResultViewModel<MenuItemDto>.NotFound("Menu item not found",404);
            }
            var data = _mapper.Map<MenuItemDto>(existedItems);
            return ResultViewModel<MenuItemDto>.Success(data, "Menu item retrieved successfully", 200);
        }

        public async Task<ResultViewModel<List<MenuItemDto>>> GetMenuItemsByCategory(string category)
        {
            var menuItems =  await _baseRepository.GetAsync(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            var data = _mapper.Map<List<MenuItemDto>>(menuItems);
            return ResultViewModel<List<MenuItemDto>>.Success(data, "Menu items retrieved successfully", 200);
        }

        public async Task<ResultViewModel<List<MenuItemDto>>> GetMenuItemsByPriceRange(decimal minPrice, decimal maxPrice)
        {
           if( minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                return ResultViewModel<List<MenuItemDto>>.Failure("Invalid price range", null, 400);
            }
            
           var existedItems= await _baseRepository.GetAsync(x => x.Price >= minPrice && x.Price <= maxPrice);
            var data = _mapper.Map<List<MenuItemDto>>(existedItems);
            return ResultViewModel<List<MenuItemDto>>.Success(data, "Menu items retrieved successfully", 200);
        }

        public async Task<ResultViewModel<List<MenuItemDto>>> SearchMenuItems(string searchTerm)
        {
            var existedItems = await _baseRepository.GetAsync(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm));
            if (existedItems == null)
            {
                return ResultViewModel<List<MenuItemDto>>.NotFound("No menu items found matching the search term",404);
            }
            var data = _mapper.Map<List<MenuItemDto>>(existedItems);
            return ResultViewModel<List<MenuItemDto>>.Success(data, "Menu items retrieved successfully", 200);
        }

        public async Task<ResultViewModel<MenuItemDto>> UpdateMenuItem(int id, UpdateMenuItemDto updateMenuItemDto)
        {
            var existedItem =  await _baseRepository.GetByIdAsync(id);
            if (existedItem == null)
            {
                return ResultViewModel<MenuItemDto>.NotFound("Menu item not found",404);
            }
            _mapper.Map(updateMenuItemDto, existedItem);

            await _baseRepository.UpdateAsync(existedItem);
            var data = _mapper.Map<MenuItemDto>(existedItem);
            return ResultViewModel<MenuItemDto>.Success(data, "Menu item updated successfully", 200);
        }
    }
}
