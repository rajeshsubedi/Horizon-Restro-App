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
    public partial class ForgotPasswordPage : ContentPage
    {
        //public class mysqlList
        //{
        //    public string username { get; set; }
        //    public string password { get; set; }
        //    public string conformpassword { get; set; }
        //}


        SqlConnection sqlConnection;

        public ForgotPasswordPage(string parameter)
        {
            InitializeComponent();
            txtUserName.Text = parameter;
            string srvrdbname = "mydb";
            string srvrname = "192.168.1.71";
            string srvrusername = "Rajesh";
            string srvrpassword = "12345";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //{
            //    try
            //    {
            //        sqlConnection.Open();

            //        string username = txtUserName.Text;
            //        string passwordTobeUpdated = txtPassword.Text;
            //        string conformTobeUpdated = txtConformPassword.Text;
            //        string qerystr = $"UPDATE dbo.mylogin SET password ='{conformTobeUpdated}' WHERE username='{username}'";

            //        using (SqlCommand command = new SqlCommand(qerystr, sqlConnection))
            //        {
            //            command.ExecuteNonQuery();
            //            sqlConnection.Close();
            //        }
            //        await App.Current.MainPage.DisplayAlert("Alert", "Congrats you have successfully changed password", "Ok");
            //        _ = Navigation.PushAsync(new LoginPage());

            //    }
            //    catch (Exception ex)
            //    {
            //        await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            //        throw;
            //    }
            //}


            //App.Current.MainPage.DisplayAlert("Alert", $"SELECT  FROM dbo.logindb WHERE username ={txtUsername.Text} and password = {txtPassword.Text}", "Ok");
            //Console.WriteLine($"SELECT  FROM dbo.logindb WHERE username={txtUsername.Text} and password={txtPassword.Text}");
            try
            {

                sqlConnection.Open();
                string username = txtUserName.Text;
                string passwordTobeUpdated = txtPassword.Text;
                string conformTobeUpdated = txtConformPassword.Text;
                using (var sqlCommand = new SqlCommand($"UPDATE dbo.mylogin SET password ='{conformTobeUpdated}' WHERE username='{username}'", sqlConnection))
                {
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (String.Equals(passwordTobeUpdated,conformTobeUpdated))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Login Successful,Password Changed.", "Ok");
                        _ = Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", " Password not matched.", "Ok");
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

        private bool stringcomapare(string passwordTobeUpdated, string conformTobeUpdated)
        {
            throw new NotImplementedException();
        }
    }
}