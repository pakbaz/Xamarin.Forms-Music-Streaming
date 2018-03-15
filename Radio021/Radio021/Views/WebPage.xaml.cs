using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Radio021.Views
{
    public partial class WebPage : ContentPage
    {
        public WebPage()
        {
            InitializeComponent();
            WebPageViewModel.IsBusy = true;
            Browser.WidthRequest = App.ScreenWidth;
            Browser.HeightRequest = App.ScreenHeight;
            Browser.Navigating += OnNavigating;
            Browser.Navigated += OnNavigated;
        }


        void OnNavigating(object sender, Xamarin.Forms.WebNavigatingEventArgs e)
        {
            WebPageViewModel.IsBusy = true;
            Task.Run(async() => await Progress.ProgressTo(0.9, 1000, Easing.SpringIn));
        }

        void OnNavigated(object sender, Xamarin.Forms.WebNavigatedEventArgs e)
        {
            WebPageViewModel.IsBusy = false;
        }
    }
}
