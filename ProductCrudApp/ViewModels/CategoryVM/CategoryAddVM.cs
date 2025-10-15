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
    public class CategoryAddVM : INotifyPropertyChanged
    {
        private readonly CategoryService _categoryService;

        private string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateCategoryCommand { get; }

        public CategoryAddVM()
        {
            _categoryService = new CategoryService();
            CreateCategoryCommand = new Command(async () => await CreateCategoryAsync());
        }

        private async Task CreateCategoryAsync()
        {
            try
            {
                var category = new Category
                {
                    Name = Name
                };

                bool isSuccess = await _categoryService.AddCategory(category);
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
