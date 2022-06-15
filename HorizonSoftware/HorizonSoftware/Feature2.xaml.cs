using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Feature2 : ContentPage
    {
        public class mysqlList
        {
            public string URL { get; set; }
            public string ItemName { get; set; }
            public string Quantity { get; set; }
            public string Price { get; set; }
            public string ID { get; set; }
            public string Total { get; set; }
            public string remarksConfirmed { get; set; }

        }
        public ObservableCollection<string> mysqlLists { get; set; }

        SqlConnection sqlConnection;
        public Feature2()
        {
            InitializeComponent();

            string srvrdbname = "mydb";
            string srvrname = "192.168.1.69";
            string srvrusername = "Rajesh";
            string srvrpassword = "samsung@M51";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);

            sqlConnection.Open();
            List<mysqlList> mysqlLists = new List<mysqlList>();
            SqlCommand command = new SqlCommand("select * from dbo.Items", sqlConnection);
            SqlDataAdapter SQLDA = new SqlDataAdapter(command);
            DataSet SQLDS = new DataSet();
            SQLDA.Fill(SQLDS);
            //if (SQLDS.Tables[0].Rows.Count > 0)
            //{
          
                //foreach (DataRow prw in SQLDS.Tables[0].Rows)
                //{
                int j = SQLDS.Tables[0].Rows.Count;
                int i;
                for (i = 0; i < j; i++)
                    {
                        mysqlLists.Add(new mysqlList
                        {                                        
                            URL = Convert.ToString(SQLDS.Tables[0].Rows[i]["URL"]),
                            ItemName = Convert.ToString(SQLDS.Tables[0].Rows[i]["ItemName"]),
                            Price = Convert.ToString(SQLDS.Tables[0].Rows[i]["Price"]),
                        } );
                    }
                //}
                sqlConnection.Close();
                MyListView.ItemsSource = mysqlLists;
            //}
        }
}
}