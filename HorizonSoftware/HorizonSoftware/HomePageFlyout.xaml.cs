using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HorizonSoftware
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageFlyout : ContentPage
    {
        ViewCell lastCell;
        public ListView ListView;

        public HomePageFlyout()
        {
            InitializeComponent();

            BindingContext = new HomePageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class HomePageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomePageFlyoutMenuItem> MenuItems { get; set; }

            public HomePageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<HomePageFlyoutMenuItem>(new[]
                {
                    new HomePageFlyoutMenuItem { Id = 0, Title = "Feature1" , IconSource="comp1.png" ,TargetType=typeof(Feature1) },
                    new HomePageFlyoutMenuItem { Id = 1, Title = "Feature2" , IconSource="comp1.png" ,TargetType=typeof(Feature2)},
                    new HomePageFlyoutMenuItem { Id = 2, Title = "Feature3" , IconSource="comp1.png" ,TargetType=typeof(Feature3)},
                    new HomePageFlyoutMenuItem { Id = 3, Title = "Feature4" , IconSource="comp1.png" ,TargetType=typeof(Feature4)},
                    new HomePageFlyoutMenuItem { Id = 4, Title = "Feature5" ,IconSource="comp1.png" ,TargetType=typeof(Feature5)},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.Orange;
                lastCell = viewCell;
            }
        }
    }
}