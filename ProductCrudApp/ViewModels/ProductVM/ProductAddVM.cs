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
    public class ProductAddVM : INotifyPropertyChanged
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        private string name = string.Empty;
        public string Name
        {
            get => name;
            set { 
                name = value; 
                OnPropertyChanged();
            }
        }

        private Category? selectedCategory;
        public Category? SelectedCategory
        {
            get => selectedCategory;
            set { selectedCategory = value; 
                OnPropertyChanged(); 
            }
        }

        private string description = string.Empty;
        public string Description
        {
            get => description;
            set { description = value; OnPropertyChanged(); }
        }

        private decimal price;
        public decimal Price
        {
            get => price;
            set { price = value; OnPropertyChanged(); }
        }

        private string selectImageText = "Select Image";
        public string SelectImageText
        {
            get => selectImageText;
            set { selectImageText = value; OnPropertyChanged(); }
        }

        private FileResult? selectedImageFile;

        public ICommand SelectImageCommand { get; }
        public ICommand CreateProductCommand { get; }

        public ProductAddVM()
        {
            _productService = new ProductService();
            _categoryService = new CategoryService();
            LoadCategories();
            SelectImageCommand = new Command(async () => await PickImageAsync());
            CreateProductCommand = new Command(async () => await CreateProductAsync());
        }

        private async void LoadCategories()
        {
            Categories.Clear();
            IEnumerable<Category> categories = await _categoryService.GetCategories();
            foreach (Category category in categories) { 
                Categories.Add(category);
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

        private async Task CreateProductAsync()
        {
            try { 
                var product = new Product
                {
                    Name = Name,
                    CategoryId = SelectedCategory?.Id ?? 0,
                    Image = string.IsNullOrEmpty(selectedImageFile?.FullPath) ? null : Path.GetFileName(selectedImageFile.FullPath),
                    Description = Description,
                    Price = Price 
                };

                bool isSuccess = await _productService.AddProduct(product, selectedImageFile);
                SnackbarHelper.ShowSnackbar(isSuccess, "Created successfully");
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
