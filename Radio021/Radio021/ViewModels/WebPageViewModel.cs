using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Radio021.ViewModels
{
    public class WebPageViewModel : BaseViewModel
    {
        public ICommand NavigatingCommand => new Command(() => IsBusy = true);
        public ICommand NavigatedCommand => new Command(() => IsBusy = false);
    }
}
