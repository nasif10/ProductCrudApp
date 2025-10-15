using ProductCrudApp.Models;
using ProductCrudApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProductCrudApp.ViewModels.CategoryVM
{
    [QueryProperty(nameof(Id), "Id")]
    public class CategoryEditVM : INotifyPropertyChanged
    {
        private readonly CategoryService _categoryService;

        private int _id;
        public string Id
        {
            get => _id.ToString();
            set
            {
                if (int.TryParse(value, out int id))
                {
                    _id = id;
                    LoadCategory(id);
                }
            }
        }

        private Category _category = new Category();
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public ICommand UpdateCategoryCommand { get; }

        public CategoryEditVM()
        {
            _categoryService = new CategoryService();
            UpdateCategoryCommand = new Command(async () => await UpdateCategoryAsync());
        }

        private async void LoadCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetCategory(id);
                if (category != null)
                {
                    Category = category;
                }
            }
            catch (Exception ex)
            {
                SnackbarHelper.ShowSnackbar(false, ex.Message);
            }
        }

        private async Task UpdateCategoryAsync()
        {
            try
            {
                var category = new Category
                {
                    Id = Category.Id,
                    Name = Category.Name,
                };

                bool isSuccess = await _categoryService.UpdateCategory(category);
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
