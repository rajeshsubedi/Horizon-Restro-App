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
        //ViewCell lastCell;

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
                    new HomePageFlyoutMenuItem { Id = 0, Title = "HomePage" , IconSource="Home.png" ,TargetType=typeof(HomePageDetail)},
                    new HomePageFlyoutMenuItem { Id = 1, Title = "Feature2" , IconSource="Comp.png" ,TargetType=typeof(Feature2)},
                    new HomePageFlyoutMenuItem { Id = 2, Title = "Feature3" , IconSource="Comp.png" ,TargetType=typeof(Feature3)},
                    new HomePageFlyoutMenuItem { Id = 3, Title = "Feature4" , IconSource="Comp.png" ,TargetType=typeof(Feature4)},
                    new HomePageFlyoutMenuItem { Id = 4, Title = "LogOut" ,IconSource="Logout.png" , TargetType=typeof(HomePageDetail)},
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


        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");

                if (result == true)
                {

                    _ = Navigation.PushAsync(new LoginPage());
                }
            });
            return true;
        }


        private async void MenuItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (e.Item == null)
            {
                return;
            }
            var selectedItem = (HomePageFlyoutMenuItem)e.Item;
            if (selectedItem.Title == "LogOut")
            {
               var result = await DisplayAlert("Alert!", "Do you really want to LogOut?", "Yes", "No");
                if (result)
                {
                    _ = Navigation.PushAsync(new LoginPage()); return;
                }
            }
        }
    }
}