using System;
using System.Collections.Generic;
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
        public AmountView(string title1, string title2, string title3)
        {
            InitializeComponent();
            Label1.Text = title1;
            Label2.Text = title2;
            Label3.Source = title3;
        }
    }
}