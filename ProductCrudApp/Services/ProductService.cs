using ProductCrudApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProductCrudApp.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private string baseUrl = ApiConfig.baseUrl + "Product/";
        public ProductService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<IEnumerable<Product>>($"{baseUrl}getProducts");
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Product> GetProduct(int id)
        {
            try
            {
                var product = await _httpClient.GetFromJsonAsync<Product>($"{baseUrl}getProduct/{id}");
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddProduct(Product product, FileResult? imageFile)
        {
            try
            {
                using (var form = new MultipartFormDataContent()) 
                { 
                    form.Add(new StringContent(product.Name), "Name");
                    form.Add(new StringContent(product.CategoryId.ToString()), "CategoryId");
                    if (!string.IsNullOrEmpty(product.Image))
                        form.Add(new StringContent(product.Image), "Image");
                    form.Add(new StringContent(product.Description), "Description");
                    form.Add(new StringContent(product.Price.ToString()), "Price");

                    if (imageFile != null)
                    {
                        var stream = await imageFile.OpenReadAsync();
                        var streamContent = new StreamContent(stream);
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                        form.Add(streamContent, "image", imageFile.FileName);
                    }

                    HttpResponseMessage response = await _httpClient.PostAsync($"{baseUrl}addProduct", form);
                    return response.IsSuccessStatusCode ? response.IsSuccessStatusCode : throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateProduct(Product product, FileResult? imageFile)
        {
            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(product.Id.ToString()), "Id");
                    form.Add(new StringContent(product.Name), "Name");
                    form.Add(new StringContent(product.CategoryId.ToString()), "CategoryId");
                    if (!string.IsNullOrEmpty(product.Image))
                        form.Add(new StringContent(product.Image), "Image");
                    form.Add(new StringContent(product.Description), "Description");
                    form.Add(new StringContent(product.Price.ToString()), "Price");

                    if (imageFile != null)
                    {
                        var stream = await imageFile.OpenReadAsync();
                        var streamContent = new StreamContent(stream);
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                        form.Add(streamContent, "image", imageFile.FileName);
                    }

                    HttpResponseMessage response = await _httpClient.PutAsync($"{baseUrl}updateProduct", form);
                    return response.IsSuccessStatusCode ? response.IsSuccessStatusCode : throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{baseUrl}deleteProduct/{id}");
                return response.IsSuccessStatusCode ? response.IsSuccessStatusCode : throw new Exception(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
