using ProductCrudApp.ViewModels.ProductVM;

namespace ProductCrudApp.Views.Product;

public partial class ProductAddPage : ContentPage
{
	public ProductAddPage()
	{
		InitializeComponent();
        BindingContext = new ProductAddVM();
    }
}