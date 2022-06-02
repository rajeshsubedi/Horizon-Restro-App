using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsView : ContentPage
    {
        public string Result { get; set; }
   

        public class mysqlList
        {
            public string URL { get; set; }
            public string ItemName { get; set; }
            public string Quantity { get; set; }
            public string Price { get; set; }
            public string ID { get; set; }
        
        }
        public ObservableCollection<string> mysqlLists { get; set; }
        public string ItemName { get; private set; }

        ObservableCollection<string> data = new ObservableCollection<string>();

        SqlConnection sqlConnection;
        public ItemsView(string title1, string title2, string title3)
        {
                InitializeComponent();
                Label1.Text = title1;
                Label2.Text = title2;
                Label3.Source = title3;

                string srvrdbname = "mydb";
                string srvrname = "192.168.1.71";
                string srvrusername = "Rajesh";
                string srvrpassword = "12345";
                string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
                sqlConnection = new SqlConnection(sqlconn);

                List<mysqlList> mysqlLists = new List<mysqlList>();
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("select * from dbo.Items", sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    mysqlLists.Add(new mysqlList
                    {
                        URL = reader["URL"].ToString(),
                        ItemName = reader["ItemName"].ToString(),
                        Price = reader["Price"].ToString(),
                

                    }
                      );

                }
                reader.Close();
                //reader.Dispose();
                sqlConnection.Close();
                MyListView.ItemsSource = mysqlLists;
                //MyListView1.ItemsSource = mysqlLists;           
        }



        private async void ItemsSearchTable_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                List<mysqlList> mysqlLists = new List<mysqlList>();
                sqlConnection.Open();
                string queryString = $"select URL,ItemName,Price from dbo.Items Where ItemName like '%{e.NewTextValue}%'";
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    mysqlLists.Add(new mysqlList
                    {
                        URL = reader["URL"].ToString(),
                        ItemName = reader["ItemName"].ToString(), 
                        Price = reader["Price"].ToString(),
                    }
                    );
                }
                reader.Close();
                sqlConnection.Close();
                MyListView.ItemsSource = mysqlLists;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }
        }



        private async void MyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
                if (e.Item == null)
                {
                    return;
                }
                var selectedItem = (mysqlList)e.Item;
                object result = await Navigation.ShowPopupAsync(new QuantityPopup(selectedItem.ItemName));
                if (result == null)
                {
                    return;
                }
                else
                Result = Convert.ToString(result);


                if (Result == "")

                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Please enter Quantity ", "Ok");
                    return;
                }
                

                else
                {
                    try
                    {
                        var res = Convert.ToInt32(Result);
                    }
                    catch(Exception)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Please enter right quantity.", "Ok");
                        return;
                    }
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand($"INSERT into ItemsSelect (ItemName, Quantity, Price,TableName,TableNo,Total) VALUES(@ItemName, @Quantity, @Price,@TableName,@TableNo,@Total)", sqlConnection))
                    {
                        command.Parameters.Add(new SqlParameter("ItemName", selectedItem.ItemName));
                        command.Parameters.Add(new SqlParameter("Quantity", Result));
                        command.Parameters.Add(new SqlParameter("Price", selectedItem.Price));
                        command.Parameters.Add(new SqlParameter("TableName", Label2.Text));
                        command.Parameters.Add(new SqlParameter("TableNo", Label1.Text));

                        int quantity = Convert.ToInt32(Result);
                        int price = Convert.ToInt32(selectedItem.Price);
                        int total = quantity * price;
                        string Total = total.ToString();
                        command.Parameters.Add(new SqlParameter("Total", Total));
                        command.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                   
                }
       
        }





        private void TabViewItem_TabTapped(object sender, Xamarin.CommunityToolkit.UI.Views.TabTappedEventArgs e)
        {
            List<mysqlList> mysqlLists = new List<mysqlList>();
            sqlConnection.Open();
            SqlCommand command = new SqlCommand($"select ItemName,Quantity,Price,ID from dbo.ItemsSelect WHERE TableName='{Label2.Text}'", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                mysqlLists.Add(new mysqlList
                {
                    ItemName = reader["ItemName"].ToString(),
                    Quantity = reader["Quantity"].ToString(),
                    Price = reader["Price"].ToString(),
                    ID = reader["ID"].ToString(),
                }
                  ); ;

            }
            reader.Close();
            //reader.Dispose();
            sqlConnection.Close();
            myCollectionView.ItemsSource = mysqlLists;
        }




        private async void Edit_Invoked_1(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var emp = item.CommandParameter as mysqlList;
            var result =  await Navigation.ShowPopupAsync(new QuantityPopup(emp.ItemName,emp.Quantity));
            if (result == null)
            {
                return;
            }
            Result = Convert.ToString(result);
            if (Result == "")

            {
                await App.Current.MainPage.DisplayAlert("Alert", "Please enter Quantity ", "Ok");
                return;
            }     
            else
            {
                try
                {
                    var res = Convert.ToInt32(Result);
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Please enter right quantity.", "Ok");
                    return;
                }
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand($"UPDATE ItemsSelect set Quantity=@Quantity,Total=@Total where TableNO=@TableNo and ID=@ID", sqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("ID", emp.ID));
                    command.Parameters.Add(new SqlParameter("Quantity", Result));           
                    command.Parameters.Add(new SqlParameter("TableNo", Label1.Text));
                    int quantity = Convert.ToInt32(Result);
                    int price = Convert.ToInt32(emp.Price);
                    int total = quantity * price;
                    string Total = total.ToString();
                    command.Parameters.Add(new SqlParameter("Total", Total));
                    command.ExecuteNonQuery();
                }



                List<mysqlList> mysqlLists = new List<mysqlList>();            
                SqlCommand command1 = new SqlCommand($"select ItemName,Quantity,Price,ID from dbo.ItemsSelect WHERE TableName='{Label2.Text}'", sqlConnection);
                SqlDataReader reader = command1.ExecuteReader();
                while (reader.Read())
                {
                    mysqlLists.Add(new mysqlList
                    {
                        ItemName = reader["ItemName"].ToString(),
                        Quantity = reader["Quantity"].ToString(),
                        Price = reader["Price"].ToString(),
                        ID = reader["ID"].ToString(),

                    }
                      ); ;

                }
                reader.Close();
                //reader.Dispose();
              
                myCollectionView.ItemsSource = mysqlLists;
                sqlConnection.Close();

            }
        }




        private void ItemsTapped_TabTapped(object sender, Xamarin.CommunityToolkit.UI.Views.TabTappedEventArgs e)
        {
            List<mysqlList> mysqlLists = new List<mysqlList>();
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("select * from dbo.Items", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                mysqlLists.Add(new mysqlList
                {
                    URL = reader["URL"].ToString(),
                    ItemName = reader["ItemName"].ToString(),
                    Price = reader["Price"].ToString(),

                }
                  );
            }
            reader.Close();
            sqlConnection.Close();
            MyListView.ItemsSource = mysqlLists;
        }




        private async void Delete_Invoked_1(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var emp = item.CommandParameter as mysqlList;
            var result = await this.DisplayAlert("Alert!", "Do you want to Delete?", "yes", "No");
            if (result == true)
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand($"Delete from ItemsSelect where TableNO=@TableNo and ID=@ID", sqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("ID", emp.ID));
                    command.Parameters.Add(new SqlParameter("TableNo", Label1.Text));
                    command.ExecuteNonQuery();
                }

                List<mysqlList> mysqlLists = new List<mysqlList>();
                SqlCommand command1 = new SqlCommand($"select ItemName,Quantity,Price,ID from dbo.ItemsSelect WHERE TableName='{Label2.Text}'", sqlConnection);
                SqlDataReader reader = command1.ExecuteReader();
                while (reader.Read())
                {
                    mysqlLists.Add(new mysqlList
                    {
                        ItemName = reader["ItemName"].ToString(),
                        Quantity = reader["Quantity"].ToString(),
                        Price = reader["Price"].ToString(),
                        ID = reader["ID"].ToString(),
                    }
                    ); ;

                }
                reader.Close();
                //reader.Dispose();
                myCollectionView.ItemsSource = mysqlLists;
                sqlConnection.Close();
            }
            else
            {
                return;
            }
        }



        private async void ClearAll_Clicked(object sender, EventArgs e)
        {
            var result = await this.DisplayAlert("Alert!", "Do you want to Delete?", "yes", "No");
            if (result == true)
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand($"Delete from ItemsSelect where TableNO=@TableNo and TableName=@TableName", sqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("TableName", Label2.Text));
                    command.Parameters.Add(new SqlParameter("TableNo", Label1.Text));
                    command.ExecuteNonQuery();
                }

                //reader.Dispose();
                myCollectionView.ItemsSource = mysqlLists;
                sqlConnection.Close();
            }
            else
            {
                return;
            }
        }
    }
}