using System;
using System.Collections.Generic;
using Radio021.Models;
using Xamarin.Forms;

namespace Radio021
{
    public partial class MainPage : MasterDetailPage
    {

        public MainPage()
        {
            InitializeComponent();
            listView.ItemSelected += OnItemSelected;
            social_twitter.Clicked += (sender, e) => Device.OpenUri(new Uri("https://www.twitter.com/"));
            social_youtube.Clicked += (sender, e) => Device.OpenUri(new Uri("https://www.youtube.com/c/"));
            social_facebook.Clicked += (sender, e) => Device.OpenUri(new Uri("https://www.facebook.com/"));
            social_telegram.Clicked += (sender, e) => Device.OpenUri(new Uri("https://t.me/"));
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                listView.SelectedItem = null;
                IsPresented = false;
            }
        }

    }
}
