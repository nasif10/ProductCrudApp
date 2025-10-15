using ProductCrudApp.Models;
using ProductCrudApp.Services;
using ProductCrudApp.Views.Category;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProductCrudApp.ViewModels.CategoryVM
{
    public class CategoryListVM : INotifyPropertyChanged
    {
        private readonly CategoryService _categoryService;

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

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

        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public CategoryListVM()
        {
            _categoryService = new CategoryService();
            LoadCategories();
            RefreshCommand = new Command(async () => await RefreshData());
            AddCommand = new Command(async () => await GoToCategoryAddPage());
            EditCommand = new Command<int>(GoToCategoryEditPage);
            DeleteCommand = new Command<int>(async (id) => await DeleteCategoryAsync(id));
        }

        public async void LoadCategories()
        {
            try
            {
                IsBusy = true;
                var categories = await _categoryService.GetCategories();
                Categories.Clear();
                foreach (var category in categories) {
                    Categories.Add(category); 
                }
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
            try
            {
                IsRefreshing = true;
                var categories = await _categoryService.GetCategories();
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
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

        private async Task GoToCategoryAddPage()
        {
            await Shell.Current.GoToAsync(nameof(CategoryAddPage));
        }

        private async void GoToCategoryEditPage(int id)
        {
            await Shell.Current.GoToAsync(nameof(CategoryEditPage) + "?Id=" + id);
        }

        private async Task DeleteCategoryAsync(int id)
        {
            try
            {
                bool confirm = await Shell.Current.DisplayAlert("Confirm Delete", "Are you sure you want to delete this item?", "Yes", "No");
                if (!confirm)
                    return;

                bool isSuccess = await _categoryService.DeleteCategory(id);
                LoadCategories();
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
