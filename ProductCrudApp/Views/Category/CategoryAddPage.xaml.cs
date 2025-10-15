using ProductCrudApp.ViewModels.CategoryVM;

namespace ProductCrudApp.Views.Category;

public partial class CategoryAddPage : ContentPage
{
	public CategoryAddPage()
	{
		InitializeComponent();
        BindingContext = new CategoryAddVM();
    }
}