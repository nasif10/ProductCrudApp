using ProductCrudApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProductCrudApp.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private string baseUrl = ApiConfig.baseUrl + "Category/";
        public CategoryService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            try
            {
                var categories = await _httpClient.GetFromJsonAsync<IEnumerable<Category>>($"{baseUrl}getCategories");
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Category> GetCategory(int id)
        {
            try
            {
                var category = await _httpClient.GetFromJsonAsync<Category>($"{baseUrl}getCategory/{id}");
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddCategory(Category category)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{baseUrl}addCategory", category);
                return response.IsSuccessStatusCode ? response.IsSuccessStatusCode : throw new Exception(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"{baseUrl}updateCategory", category);
                return response.IsSuccessStatusCode ? response.IsSuccessStatusCode : throw new Exception(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{baseUrl}deleteCategory/{id}");
                return response.IsSuccessStatusCode ? response.IsSuccessStatusCode : throw new Exception(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
