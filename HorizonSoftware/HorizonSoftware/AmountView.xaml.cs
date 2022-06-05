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
    public partial class AmountView : ContentPage
    {
        public string Result { get; set; }

        public class mysqlList
        {
            public string URL { get; set; }
            public string ItemName { get; set; }
            public string Price { get; set; }
            public string Quantity { get; set; }
            public string Total { get; set; }
            public string ID { get; set; }
        }

        public ObservableCollection<string> mysqlLists { get; set; }

        SqlConnection sqlConnection;
        public AmountView(string title1, string title2, string title3)
        {
            InitializeComponent();
            Label1.Text = title1;
            Label2.Text = title2;
            Label3.Source = title3;

            string srvrdbname = "mydb";
            string srvrname = "192.168.1.74";
            string srvrusername = "Rajesh";
            string srvrpassword = "samsung@M51";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";

            sqlConnection = new SqlConnection(sqlconn);

            List<mysqlList> mysqlLists = new List<mysqlList>();
            sqlConnection.Open();
            SqlCommand command = new SqlCommand($"select ItemName,Price,Quantity,Total from dbo.ItemsSelect WHERE TableName='{Label2.Text}' and TableNo='{Label1.Text}' Union all select Item='Total',Price='',Quantity=sum(Quantity),Total=sum(Total) from dbo.ItemsSelect WHERE TableName='{Label2.Text}' and TableNo='{Label1.Text}'", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                mysqlLists.Add(new mysqlList
                {
                    ItemName = reader["ItemName"].ToString(),                 
                    Price = reader["Price"].ToString(),
                    Quantity = reader["Quantity"].ToString(),
                    Total = reader["Total"].ToString(),
                  
                }
                  ); ;

            }
            reader.Close();
            //reader.Dispose();
            sqlConnection.Close();
            myCollectionView.ItemsSource = mysqlLists;
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