using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HorizonSoftware
{
    public partial class LoginPage : ContentPage

    {
        SqlConnection sqlConnection;

        public LoginPage()
        {
            InitializeComponent();
            string srvrdbname = "mydb";
            string srvrname = "192.168.1.69";
            string srvrusername = "Rajesh";
            string srvrpassword = "samsung@M51";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
            Close.Clicked += Button_Clicked_1;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            {
                Console.WriteLine($"SELECT username From dbo.mylogin WHERE username='{txtUsername.Text}' and password='{txtPassword.Text}'");
                try
                {
                    sqlConnection.Open();
                    using (var sqlCommand = new SqlCommand($"SELECT username From dbo.mylogin WHERE username='{txtUsername.Text}' and password='{txtPassword.Text}'", sqlConnection))
                    { 
                            SqlDataReader reader = sqlCommand.ExecuteReader();
                        if (reader.Read())
                        {
                            _ = Navigation.PushAsync(new HomePage());
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "Username and Password is incorrect.", "Ok");
                        }

                        reader.Close();
                        reader.Dispose();
                    }

                    sqlConnection.Close();


                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                    throw;
                }

            }

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //{
            //    Navigation.PushAsync(new SignupPage());
            //}
        }


        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ForgotPasswordPage(txtUsername.Text));
        }


        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var result = await this.DisplayAlert("Alert!", "Do you want to exit?", "yes", "No");
            if (result)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); // Or anything else
            }
        }

        //System.Environment.Exit(0);
        //protected override bool OnBackButtonPressed()
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        var result = await this.DisplayAlert("Alert!", "Do you want to exit?", "yes", "No");
        //        if (result) await this.Navigation.PopAsync();
        //    });
        //    //base.OnBackButtonPressed();
        //    return true;
        //}

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");

                if (result)
                {
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); // Or anything else
                }
            });
            return true;
        }
    }
}
