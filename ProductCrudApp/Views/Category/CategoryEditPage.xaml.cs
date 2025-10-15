using ProductCrudApp.ViewModels.CategoryVM;

namespace ProductCrudApp.Views.Category;

public partial class CategoryEditPage : ContentPage
{
	public CategoryEditPage()
	{
		InitializeComponent();
        BindingContext = new CategoryEditVM();
    }
}