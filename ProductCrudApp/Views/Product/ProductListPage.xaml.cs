using ProductCrudApp.ViewModels.ProductVM;

namespace ProductCrudApp.Views.Product;

public partial class ProductListPage : ContentPage
{
	public ProductListPage()
	{
		InitializeComponent();
        BindingContext = new ProductListVM();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is ProductListVM viewModel)
        {
            viewModel.LoadProducts();
        }
    }
}