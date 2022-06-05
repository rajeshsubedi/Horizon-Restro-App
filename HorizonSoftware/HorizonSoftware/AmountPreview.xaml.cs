using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HorizonSoftware.ActivationPage;

namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AmountPreview : ContentPage
    {
        public class mysqlList
        {
            public string URL { get; set; }
            public string TableName { get; set; }
            public string TableNo { get; set; }
        }
        public ObservableCollection<string> mysqlLists { get; set; }

        SqlConnection sqlConnection;
        public AmountPreview()
        {
            InitializeComponent();

            string srvrdbname = "mydb";
            string srvrname = "192.168.1.74";
            string srvrusername = "Rajesh";
            string srvrpassword = "samsung@M51";
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
                    TableNo = reader["TableNo"].ToString(),
                }
                  );

            }
            reader.Close();
            //reader.Dispose();
            sqlConnection.Close();
            TableView.ItemsSource = mysqlLists;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var item = sender as ImageButton;
            var emp = item.CommandParameter as mysqlList;

            Navigation.PushAsync(new AmountView(emp.TableNo,emp.TableName,emp.URL));
        }
    }
}