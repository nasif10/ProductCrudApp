using ProductCrudApp.ViewModels.ProductVM;

namespace ProductCrudApp.Views.Product;

public partial class ProductEditPage : ContentPage
{
	public ProductEditPage()
	{
		InitializeComponent();
        BindingContext = new ProductEditVM();
    }
}