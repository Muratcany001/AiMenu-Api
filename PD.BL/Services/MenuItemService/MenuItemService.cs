using AutoMapper;
using Common.ViewModels;
using Dtos.MenuItemDto;
using Dtos.UserDtos;
using FluentValidation;
using PD.BL.Helpers.GeminiHelper;
using PD.BL.Services.RedisCacheService;
using PD.DAL.Entitites.AppEntitites;
using PD.DAL.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.MenuItemService
{
    public class MenuItemService : IMenuItemService
    {
        private readonly HttpClient _httpClient;
        private readonly IBaseRepository<MenuItem> _baseRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cache;
        private readonly GeminiHelper _geminiHelper;
        public MenuItemService(IBaseRepository<MenuItem> menuItemRepository, IMapper mapper, IRedisCacheService cache, HttpClient httpClient, GeminiHelper geminiHelper)
        {
            _baseRepository = menuItemRepository;
            _mapper = mapper;
            _cache = cache;
            _httpClient = httpClient;
            _geminiHelper = geminiHelper;
        }

        public async Task<ResultViewModel<MenuItemDto>> AddMenuItem(AddMenuItemDto addMenuItemDto)
        {
            var existedItem = await _baseRepository.GetAsync(x => x.Name.Equals(addMenuItemDto.Name));
            if (existedItem != null)
            {
                return ResultViewModel<MenuItemDto>.Failure("this item already exists",null, 400);
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
            string key = "all_menu_items";
            var cache = _cache.GetData<List<MenuItemDto>>(key);
            if (cache != null)
            {
                return ResultViewModel<List<MenuItemDto>>.Success(cache, "Menu items retrieved successfully from cache", 200);
            }
            else { 

                var existedItems = await _baseRepository.GetListAsync(asNoTracking: false);
                if (existedItems == null)
                {
                    return ResultViewModel<List<MenuItemDto>>.NotFound("No menu items found",404);
                }
                var list = _mapper.Map<List<MenuItemDto>>(existedItems);
                _cache.SetData("all_menu_items",list);
                return ResultViewModel<List<MenuItemDto>>.Success(list, "Menu items retrieved successfully", 200);
            }
        }

        public async Task<ResultViewModel<MenuItemDto>> GetMenuItemById(int id)
        {
            string key = $"menu_item_{id}";
            var cachedData = _cache.GetData<MenuItemDto>(key);
            if (cachedData != null)
            {
                return ResultViewModel<MenuItemDto>.Success(cachedData, "Menu item retrieved successfully from cache", 200);
            }
            var existedItems = await _baseRepository.GetByIdAsync(id);
            if (existedItems == null)
            {
                return ResultViewModel<MenuItemDto>.NotFound("Menu item not found",404);
            }
            var data = _mapper.Map<MenuItemDto>(existedItems);
            _cache.SetData(key, data);
            return ResultViewModel<MenuItemDto>.Success(data, "Menu item retrieved successfully", 200);
        }

        public async Task<ResultViewModel<List<MenuItemDto>>> GetMenuItemByIngeredients(string ingredients)
        {
            // cache sorgusu 
            string searchTerm = ingredients.Trim().ToLower();
            string key = $"menu_items_ingredients_{searchTerm}";

            var cachedData = _cache.GetData<List<MenuItemDto>>(key);
            if (cachedData != null)
            {
                return ResultViewModel<List<MenuItemDto>>.Success(cachedData, "Menu items retrieved succesfully from cache", 200);
            }
            var existedItems = await _baseRepository.GetListAsync(x => x.Ingeredents.Equals(ingredients));
            var data = _mapper.Map<List<MenuItemDto>>(existedItems);
            //eger cache icerisinde yoksa set etme
            _cache.SetData<List<MenuItemDto>>(key, data);
            return ResultViewModel<List<MenuItemDto>>.Success(data, "Menu items retrieved successfully", 200);
        }

        public async Task<ResultViewModel<List<MenuItemDto>>> GetMenuItemsByCategory(string category)
        {
            var menuItems =  await _baseRepository.GetListAsync(x => x.Category.ToUpper() == category.ToUpper());
            var data = _mapper.Map<List<MenuItemDto>>(menuItems);
            return ResultViewModel<List<MenuItemDto>>.Success(data, "Menu items retrieved successfully", 200);
        }

        public async Task<ResultViewModel<List<MenuItemDto>>> GetMenuItemsByPriceRange(decimal minPrice, decimal maxPrice)
        {
           if( minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                return ResultViewModel<List<MenuItemDto>>.Failure("Invalid price range", null, 400);
            }
            
           var existedItems= await _baseRepository.GetListAsync(x => x.Price >= minPrice && x.Price <= maxPrice);
            var data = _mapper.Map<List<MenuItemDto>>(existedItems);
            return ResultViewModel<List<MenuItemDto>>.Success(data, "Menu items retrieved successfully", 200);
        }

        public async Task<ResultViewModel<List<MenuItemDto>>> SearchMenuItems(string searchTerm)
        {
            System.Diagnostics.Debug.WriteLine($" Arama terimi: '{searchTerm}'");

            var existedItems = await _baseRepository.GetListAsync(asNoTracking: false);

            System.Diagnostics.Debug.WriteLine($" Veritabanından {existedItems.Count()} ürün geldi");

            if (!existedItems.Any())
                return ResultViewModel<List<MenuItemDto>>
                    .NotFound("No menu items found", 404);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var allData = _mapper.Map<List<MenuItemDto>>(existedItems);
                return ResultViewModel<List<MenuItemDto>>
                    .Success(allData, "Menu items retrieved successfully", 200);
            }

            // Check cache first before calling Gemini
            var cacheKey = $"search_{searchTerm.ToLowerInvariant()}";
            var cachedResult = _cache.GetData<List<int>>(cacheKey);
            List<int> geminiResponse;

            if (cachedResult != null)
            {
                System.Diagnostics.Debug.WriteLine($" Cache hit: {cacheKey}");
                geminiResponse = cachedResult;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(" Gemini çağrılıyor...");
                geminiResponse = await _geminiHelper.CallGeminiAsync(searchTerm, existedItems);
                
                // Cache the result for 1 hour
                if (geminiResponse != null && geminiResponse.Any())
                {
                    _cache.SetData(cacheKey, geminiResponse, TimeSpan.FromHours(1));
                }
            }

            System.Diagnostics.Debug.WriteLine($" Gemini cevabı: {geminiResponse?.Count ?? 0} ID");

            List<MenuItem> filteredItems;

            if (geminiResponse != null && geminiResponse.Any())
            {
                System.Diagnostics.Debug.WriteLine($" Gemini ID'leri: [{string.Join(", ", geminiResponse)}]");

                var idSet = geminiResponse.ToHashSet();
                filteredItems = existedItems.Where(x => idSet.Contains(x.Id)).ToList();

                System.Diagnostics.Debug.WriteLine($" ID'lere göre {filteredItems.Count} ürün bulundu");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(" Gemini boş döndü, fallback string search başlıyor...");

                var searchLower = searchTerm.ToLowerInvariant();

                filteredItems = existedItems.Where(x =>
                    (x.Name ?? "").ToLowerInvariant().Contains(searchLower) ||
                    (x.Description ?? "").ToLowerInvariant().Contains(searchLower) ||
                    (x.Ingeredents ?? "").ToLowerInvariant().Contains(searchLower) ||
                    (x.Category ?? "").ToLowerInvariant().Contains(searchLower)
                ).ToList();

                System.Diagnostics.Debug.WriteLine($" String search sonucu: {filteredItems.Count} ürün");

                if (!filteredItems.Any())
                {
                    System.Diagnostics.Debug.WriteLine(" Hiçbir ürün bulunamadı");

                    var samples = existedItems.Take(3).Select(x =>
                        $"'{x.Name}' (Cat: {x.Category})").ToList();
                    System.Diagnostics.Debug.WriteLine($" Örnek ürünler: {string.Join(", ", samples)}");

                    return ResultViewModel<List<MenuItemDto>>
                        .NotFound("İsteğinize uygun ürün bulunamadı.", 404);
                }

                System.Diagnostics.Debug.WriteLine($"🔎 Bulunan ürünler: {string.Join(", ", filteredItems.Select(x => x.Name))}");
            }

            var data = _mapper.Map<List<MenuItemDto>>(filteredItems);

            System.Diagnostics.Debug.WriteLine($" Başarılı: {data.Count} ürün döndürülüyor");

            return ResultViewModel<List<MenuItemDto>>
                .Success(data, "Menu items retrieved successfully", 200);
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
