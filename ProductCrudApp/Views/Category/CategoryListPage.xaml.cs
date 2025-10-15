using ProductCrudApp.ViewModels.CategoryVM;

namespace ProductCrudApp.Views.Category;

public partial class CategoryListPage : ContentPage
{
	public CategoryListPage()
	{
		InitializeComponent();
        BindingContext = new CategoryListVM();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is CategoryListVM viewModel)
        {
            viewModel.LoadCategories();
        }
    }
}