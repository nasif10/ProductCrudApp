using ProductCrudApp.Models;
using ProductCrudApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProductCrudApp.ViewModels.ProductVM
{
    [QueryProperty(nameof(Id), "Id")]
    public class ProductEditVM : INotifyPropertyChanged
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        private int _id;
        public string Id
        {
            get => _id.ToString();
            set
            {
                if (int.TryParse(value, out int id))
                {
                    _id = id;
                    LoadProduct(id);
                }
            }
        }

        private Product _product = new Product();
        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        private Category? selectedCategory;
        public Category? SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        private string selectImageText = "Select Image";
        public string SelectImageText
        {
            get => selectImageText;
            set { selectImageText = value; OnPropertyChanged(); }
        }

        private FileResult? selectedImageFile;
        public ICommand SelectImageCommand { get; }
        public ICommand UpdateProductCommand { get; }

        public ProductEditVM()
        {
            _productService = new ProductService();
            _categoryService = new CategoryService();
            SelectImageCommand = new Command(async () => await PickImageAsync());
            UpdateProductCommand = new Command(async () => await UpdateProductAsync());
        }

        private async Task LoadCategories()
        {
            Categories.Clear();
            var categories = await _categoryService.GetCategories();
            foreach (Category category in categories)
            {
                Categories.Add(category);
            }
        }

        private async void LoadProduct(int id)
        {
            try
            {
                await LoadCategories();

                var product = await _productService.GetProduct(id);
                if (product != null) {
                    Product = product;
                    SelectedCategory = Categories.FirstOrDefault(c => c.Id == Product.CategoryId);
                }
            }
            catch (Exception ex)
            {
                SnackbarHelper.ShowSnackbar(false, ex.Message);
            }
        }

        private async Task PickImageAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    selectedImageFile = result;
                    SelectImageText = Path.GetFileName(selectedImageFile.FullPath);
                }
            }
            catch (Exception ex)
            {
                SnackbarHelper.ShowSnackbar(false, ex.Message);
            }
        }

        private async Task UpdateProductAsync()
        {
            try
            {
                var product = new Product
                {
                    Id = Product.Id,
                    Name = Product.Name,
                    CategoryId = SelectedCategory?.Id ?? 0,
                    Image = string.IsNullOrEmpty(selectedImageFile?.FullPath) ? Product.Image : Path.GetFileName(selectedImageFile.FullPath),
                    Description = Product.Description,
                    Price = Product.Price
                };

                bool isSuccess = await _productService.UpdateProduct(product, selectedImageFile);
                SnackbarHelper.ShowSnackbar(isSuccess, "Updated successfully");
            }
            catch (Exception ex)
            {
                SnackbarHelper.ShowSnackbar(false, ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
