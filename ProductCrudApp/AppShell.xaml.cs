using ProductCrudApp.Views.Category;
using ProductCrudApp.Views.Product;

namespace ProductCrudApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ProductAddPage), typeof(ProductAddPage));
            Routing.RegisterRoute(nameof(ProductEditPage), typeof(ProductEditPage));
            Routing.RegisterRoute(nameof(CategoryAddPage), typeof(CategoryAddPage));
            Routing.RegisterRoute(nameof(CategoryEditPage), typeof(CategoryEditPage));
        }
    }
}
