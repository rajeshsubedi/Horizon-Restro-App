using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data.SQLite;
using System.IO;
namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderTable : ContentPage
    {
        public class mysqlList
        {
            public string URL { get; set; }
            public string TableName { get; set; }
            public string TableNo { get; set; }
            public string Packed { get; set; }
        }

        public ObservableCollection<string> mysqlLists { get; set; }
        public string TableName { get; private set; }

        ObservableCollection<string> data = new ObservableCollection<string>();

        SqlConnection sqlConnection;
        public OrderTable()
        {
            InitializeComponent();

            string srvrdbname = "mydb";
            string srvrname = "192.168.1.72";
            string srvrusername = "Rajesh";
            string srvrpassword = "samsung@M51";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);

            List<mysqlList> mysqlLists = new List<mysqlList>();
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("select distinct Tabledb.TableNo,CAST(URL as varchar(8000))as URL,Tabledb.TableName,Confirmed  from Tabledb left join ItemsSelect on ItemsSelect.TableNo = Tabledb.TableNo ", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                mysqlLists.Add(new mysqlList
                {
                    TableNo = reader["TableNo"].ToString(),
                    URL = reader["URL"].ToString(),
                    TableName = reader["TableName"].ToString(),
                    Packed = reader["Confirmed"].ToString() != "" ? "#4284f5" : "#fff",
              
                }
                ); ;

            }
            reader.Close();
            //reader.Dispose();
            sqlConnection.Close();
            TableOrder.ItemsSource = mysqlLists;
        }


        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var item = sender as ImageButton;
            var emp = item.CommandParameter as mysqlList;      
            Navigation.PushAsync(new ItemsView(emp.TableNo, emp.TableName, emp.URL));
        }
    }

}