using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Radio021.ViewModels
{
    public class AboutPageViewModel
    {
        public ICommand OpenWebCommand => new Command(() => Device.OpenUri(new Uri("https://web.com")));
        public ICommand OpenEmailCommand => new Command(() => Device.OpenUri(new Uri("mailto:info@web.com")));
        public ICommand OpenIOA => new Command(() => Device.OpenUri(new Uri("https://example.com")));
    }
}
