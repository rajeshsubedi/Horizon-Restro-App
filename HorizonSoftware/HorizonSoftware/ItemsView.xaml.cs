﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsView : ContentPage
    {
       

        public ItemsView(string title1, string title2)
        {
            InitializeComponent();
            Label.Text = title1;
            Lable2.Source = title2;

        }

    }
}