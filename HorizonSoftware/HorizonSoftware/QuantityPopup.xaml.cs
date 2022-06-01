using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuantityPopup : Popup
    {
        public class mysqlList
        {
        
            public string Quantity { get; set; }
        }

        public ObservableCollection<string> mysqlLists { get; set; }
        public string ItemName { get; private set; }
       
        SqlConnection sqlConnection;
    

        public QuantityPopup(string title1, string quantity="")
        {
            InitializeComponent();

            Label1.Text = title1;
            Quantity.Text = quantity.ToString();

            string srvrdbname = "mydb";
            string srvrname = "192.168.1.71";
            string srvrusername = "Rajesh";
            string srvrpassword = "12345";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }



 

        private void Ok_Clicked(object sender, EventArgs e)
        {
            Dismiss(Quantity.Text);
        }
    }
}