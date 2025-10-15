using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ProductCrudApp.Services
{
    public class ApiConfig
    {
        public const string baseUrl = "http://192.168.1.109/ProductCrudAPI/api/v1/";
        public const string fileUrl = "http://192.168.1.109/ProductCrudAPI/images/";
    }

    public static class SnackbarHelper
    {
        public static async void ShowSnackbar(bool isSuccess, string text)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = isSuccess ? Colors.MediumSeaGreen : Colors.Orange,
            };

            var snackbar = Snackbar.Make(text, null, "OK", null, snackbarOptions);
            await snackbar.Show();
        }
    }
}
