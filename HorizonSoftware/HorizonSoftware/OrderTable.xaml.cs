using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderTable : ContentPage
    {
        public class mysqlList
        {
            public string URL { get; set; }
            public string TableName { get; set; }
        }

       


        public ObservableCollection<string> mysqlLists { get; set; }
        public string TableName { get; private set; }

        ObservableCollection<string> data = new ObservableCollection<string>();

        SqlConnection sqlConnection;
        public OrderTable()
        {
            InitializeComponent();
            string srvrdbname = "mydb";
            string srvrname = "192.168.1.96";
            string srvrusername = "Rajesh";
            string srvrpassword = "12345";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);

            List<mysqlList> mysqlLists = new List<mysqlList>();
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("select * from dbo.Tabledb", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                mysqlLists.Add(new mysqlList
                {
                    URL = reader["URL"].ToString(),
                    TableName = reader["TableName"].ToString(),

                }
                  );
    
            }
            reader.Close();
            //reader.Dispose();
            sqlConnection.Close();
            MyListView.ItemsSource = mysqlLists;
        }



        private async void SearchTable_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                List<mysqlList> mysqlLists = new List<mysqlList>();
                sqlConnection.Open();
                string queryString = $"select URL,TableName from dbo.Tabledb Where TableName like '%{e.NewTextValue}%'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    mysqlLists.Add(new mysqlList
                    {
                        URL = reader["URL"].ToString(),
                        TableName = reader["TableName"].ToString(),
                    }
                    );
                }
                reader.Close();
                sqlConnection.Close();
                MyListView.ItemsSource = mysqlLists;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }
        }




    

        private void MyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }

            var selectedItem = (mysqlList)e.Item; // model
            Navigation.PushAsync(new ItemsView(selectedItem.TableName,selectedItem.URL)); // pass the selected whole item from list to DetaiPage 'selectedItem' using constructor
            ((ListView)sender).SelectedItem = null;

     
        }


    }
}