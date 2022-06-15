using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HorizonSoftware
{
    public class ServerConnect : ContentPage
    {
        SqlConnection sqlConnection;
        public ServerConnect()
        {
           
            string srvrdbname = "mydb";
            string srvrname = "192.168.1.69";
            string srvrusername = "Rajesh";
            string srvrpassword = "samsung@M51";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }
    }
}