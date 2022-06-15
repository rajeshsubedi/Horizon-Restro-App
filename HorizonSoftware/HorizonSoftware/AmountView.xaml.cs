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
            string srvrname = "192.168.1.69";
            string srvrusername = "Rajesh";
            string srvrpassword = "samsung@M51";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";

            sqlConnection = new SqlConnection(sqlconn);

            List<mysqlList> mysqlLists = new List<mysqlList>();
            sqlConnection.Open();
            SqlCommand command = new SqlCommand($"select ItemName,Price,Quantity,Total from dbo.ItemsSelect WHERE TableName='{Label2.Text}' and TableNo='{Label1.Text}' and Confirmed = 1", sqlConnection);          
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

            SqlCommand command1 = new SqlCommand($"select sum(Quantity) as quantityTotal, sum(Total) as priceTotal from dbo.ItemsSelect WHERE TableName='{Label2.Text}' and TableNo='{Label1.Text}' and Confirmed = 1", sqlConnection);
            SqlDataReader reader1 = command1.ExecuteReader();
            reader1.Read();         
            quantitySum.Text = reader1["quantityTotal"].ToString();
            PriceSum.Text = reader1["priceTotal"].ToString();
            int discount = 0;
            Discount.Text = $"{discount}%";
            discountAmount.Text = "0";
            vatAmount.Text = "0";
            chargeAmount.Text = "0";

            if (PriceSum.Text != "")
            {
                int total = Convert.ToInt32(PriceSum.Text) + Convert.ToInt32(discountAmount.Text) + Convert.ToInt32(vatAmount.Text) + Convert.ToInt32(chargeAmount.Text);
                totalAmount.Text = total.ToString();
            }
            else
            {
                totalAmount.Text = "0";
            }
            //evaluateTotal();
          
            reader1.Close();
            sqlConnection.Close();
            myCollectionView.ItemsSource = mysqlLists;
        }




        private void evaluateTotal()
        {
            int price = Convert.ToInt32(PriceSum.Text);
            int discount = Convert.ToInt32(Discount.Text.ToString().Replace("%", ""));
            int disAmount = Convert.ToInt32(discount * 0.01 * price);
            discountAmount.Text = disAmount.ToString();
            int vatamount = 0 ;
            if (vatClicked.IsChecked)
            {
                vatamount = Convert.ToInt32((price - disAmount) * 0.13);
            }
            vatAmount.Text = vatamount.ToString();

            int serviceamount = 0;
            if (chargeClicked.IsChecked)
            {
                serviceamount = Convert.ToInt32((price - disAmount) * 0.10);

            }
            chargeAmount.Text = serviceamount.ToString();

            totalAmount.Text = (price - disAmount + vatamount + serviceamount).ToString();

        }


        private async void vatClicked_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            int discount = Convert.ToInt32(Discount.Text.ToString().Replace("%", ""));
            if (PriceSum.Text != "" && discount != 0)
            {
                evaluateTotal();
            }
            else if (PriceSum.Text != "" && discount == 0)
            {
                int price = Convert.ToInt32(PriceSum.Text);
                int disAmount = Convert.ToInt32(discountAmount.Text);

                int vatamount = 0;
                if (vatClicked.IsChecked)
                {
                    vatamount = Convert.ToInt32((price - disAmount) * 0.13);
                }
                vatAmount.Text = vatamount.ToString();

                int serviceamount = 0;
                if (chargeClicked.IsChecked)
                {
                    serviceamount = Convert.ToInt32((price - disAmount) * 0.10);

                }
                chargeAmount.Text = serviceamount.ToString();

                totalAmount.Text = (price - disAmount + vatamount + serviceamount).ToString();

            }
            else
            {
                await DisplayAlert("Alert", "Please enter Items ", "Ok");
            }
        }



        private async void chargeClicked_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            int discount = Convert.ToInt32(Discount.Text.ToString().Replace("%", ""));
            if (PriceSum.Text != ""  && discount != 0)
            {
                evaluateTotal();
            }
            else if(PriceSum.Text != "" && discount == 0)
             {
                int price = Convert.ToInt32(PriceSum.Text);
                int disAmount = Convert.ToInt32(discountAmount.Text);

                int vatamount = 0;
                if (vatClicked.IsChecked)
                {
                    vatamount = Convert.ToInt32((price - disAmount) * 0.13);
                }
                vatAmount.Text = vatamount.ToString();

                int serviceamount = 0;
                if (chargeClicked.IsChecked)
                {
                    serviceamount = Convert.ToInt32((price - disAmount) * 0.10);

                }
                chargeAmount.Text = serviceamount.ToString();

                totalAmount.Text = (price - disAmount + vatamount + serviceamount).ToString();

            }
            else
            {
                await DisplayAlert("Alert", "Please enter Items ", "Ok");
            }
        }



        private void Discount_Completed(object sender, EventArgs e)
        {
            string currentValue = Discount.Text.Replace("%", "");
            if(currentValue == "")
            {
                return ;
            }
            sqlConnection.Open();
            SqlCommand command1 = new SqlCommand($"select sum(Quantity) as quantityTotal, sum(Total) as priceTotal from dbo.ItemsSelect WHERE TableName='{Label2.Text}' and TableNo='{Label1.Text}' and Confirmed = 1", sqlConnection);
            SqlDataReader reader1 = command1.ExecuteReader();
            reader1.Read();
            PriceSum.Text = reader1["priceTotal"].ToString();
            int price = Convert.ToInt32(PriceSum.Text);       
                    int discount = Convert.ToInt32(Discount.Text.ToString().Replace("%", ""));
                     int disAmount = Convert.ToInt32(discount * 0.01 * price);
                      discountAmount.Text = disAmount.ToString();             
                        evaluateTotal();
      
            reader1.Close();           
            sqlConnection.Close();
        }




        private void Discount_TextChanged(object sender, TextChangedEventArgs e)
        {
            string currentValue = e.NewTextValue.Replace("%", "");
            if (currentValue != "" && Convert.ToInt32(currentValue) >= 0 && Convert.ToInt32(currentValue) <= 100)
            {
                Discount.Text = $"{e.NewTextValue.Replace("%", "")}%";
            }
         else 
            {

                Discount.Text =  currentValue == ""? $"{e.NewTextValue.Replace("%", "")}%": $"{e.OldTextValue.Replace("%", "")}%";
            }

        }



        private void discountAmount_Completed(object sender, EventArgs e)
        {

            sqlConnection.Open();
            SqlCommand command1 = new SqlCommand($"select sum(Quantity) as quantityTotal, sum(Total) as priceTotal from dbo.ItemsSelect WHERE TableName='{Label2.Text}' and TableNo='{Label1.Text}' and Confirmed = 1", sqlConnection);
            SqlDataReader reader1 = command1.ExecuteReader();
            reader1.Read();
            PriceSum.Text = reader1["priceTotal"].ToString();
        
            int price = Convert.ToInt32(PriceSum.Text);
            int disAmount = Convert.ToInt32(discountAmount.Text);

            int vatamount = 0;
            if (vatClicked.IsChecked)
            {
                vatamount = Convert.ToInt32((price - disAmount) * 0.13);
            }
            vatAmount.Text = vatamount.ToString();

            int serviceamount = 0;
            if (chargeClicked.IsChecked)
            {
                serviceamount = Convert.ToInt32((price - disAmount) * 0.10);

            }
            chargeAmount.Text = serviceamount.ToString();

            totalAmount.Text = (price - disAmount + vatamount + serviceamount).ToString();


            reader1.Close();
            sqlConnection.Close();
        }
    }

}