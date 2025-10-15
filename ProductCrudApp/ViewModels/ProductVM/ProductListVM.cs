using CommunityToolkit.Maui.Views;
using ProductCrudApp.Models;
using ProductCrudApp.Services;
using ProductCrudApp.Views.Product;
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
    public class ProductListVM : INotifyPropertyChanged
    {
        private readonly ProductService _productService;
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        IEnumerable<Product> products = Enumerable.Empty<Product>();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }

        private string searchText = string.Empty;
        public string SearchText
        {
            get => searchText;
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    OnPropertyChanged();
                    FilterProducts();
                }
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ShowImageCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProductListVM()
        {
            _productService = new ProductService();
            RefreshCommand = new Command(async () => await RefreshData());
            ShowImageCommand = new Command<string>(OnShowImage);
            AddCommand = new Command(async () => await GoToAddProductPage());
            EditCommand = new Command<int>(GoToEditProductPage);
            DeleteCommand = new Command<int>(async (id) => await DeleteProductAsync(id));
            LoadProducts();
        }

        public async void LoadProducts()
        {
            try
            {
                IsBusy = true;
                products = await _productService.GetProducts();
                Products.Clear();
                foreach (var product in products)
                    Products.Add(product);
            }
            catch (Exception ex)
            {
                SnackbarHelper.ShowSnackbar(false, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshData()
        {
            try { 
                IsRefreshing = true;
                products = await _productService.GetProducts();
                Products.Clear();
                foreach (var product in products)
                    Products.Add(product);
            }
            catch (Exception ex)
            {
                SnackbarHelper.ShowSnackbar(false, ex.Message);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void FilterProducts()
        {
            Products.Clear();
            IEnumerable<Product> fProducts;

            if (string.IsNullOrEmpty(SearchText))
            {
                fProducts = products;
            }
            else
            {
                fProducts = products.Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var product in fProducts)
                Products.Add(product);
        }

        private void OnShowImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var stack = new VerticalStackLayout
            {
                Padding = 10,
                Children =
                {
                    new Image
                    {
                        Source = ApiConfig.fileUrl + imageUrl,
                        Aspect = Aspect.AspectFit
                    }
                }
            };

            var popup = new Popup
            {
                Size = new Size(240, 240),
                Content = stack
            };

            Shell.Current.ShowPopup(popup);
        }

        private async Task GoToAddProductPage()
        {
            await Shell.Current.GoToAsync(nameof(ProductAddPage));
        }

        private async void GoToEditProductPage(int id)
        {
            await Shell.Current.GoToAsync(nameof(ProductEditPage) + "?Id=" + id);
        }

        private async Task DeleteProductAsync(int id)
        {
            try
            {
                bool confirm = await Shell.Current.DisplayAlert("Confirm Delete", "Are you sure you want to delete this item?", "Yes", "No");
                if (!confirm)
                    return;

                bool isSuccess = await _productService.DeleteProduct(id);
                LoadProducts();
                SnackbarHelper.ShowSnackbar(isSuccess, "Deleted successfully");
            }
            catch (Exception ex)
            {
                SnackbarHelper.ShowSnackbar(false, ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
