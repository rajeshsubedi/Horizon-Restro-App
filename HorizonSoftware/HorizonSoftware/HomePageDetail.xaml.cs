using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageDetail : ContentPage
    {


        SqlConnection sqlConnection;
        public HomePageDetail()
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.Black;
            InitializeComponent();
            string srvrdbname = "mydb";
            string srvrname = "192.168.1.96";
            string srvrusername = "Rajesh";
            string srvrpassword = "12345";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private async void Close_Clicked_1(object sender, EventArgs e)
        {
            var result = await this.DisplayAlert("Alert!", "Do you want to Logout?", "yes", "No");
            if (result == true)
            {
                _ = Navigation.PushAsync(new LoginPage());
            }
            else
            {
                //_ = Navigation.PushAsync(new HomePageDetail());
            }
        }




        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");

                if (result == true)
                {
                    /*    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();*/ // Or anything else
                    _ = Navigation.PushAsync(new LoginPage());
                }
            });
            return true;
        }

        private void Ordertable_Clicked(object sender, EventArgs e)
        {
            _ = Navigation.PushAsync(new OrderTable());          
        }




        private void Amountpreview_Clicked(object sender, EventArgs e)
        {
            _ = Navigation.PushAsync(new AmountPreview());
        }
    }
}