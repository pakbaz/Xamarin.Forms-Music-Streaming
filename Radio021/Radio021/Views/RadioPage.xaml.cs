using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Radio021.Views
{
    public partial class RadioPage : CarouselPage
    {
        public RadioPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<App>(this, "metadata", (obj) => {
                Task.Run(() => AnimateLogo());
            });


        }

        async Task AnimateLogo()
        {
            await Task.WhenAll(
                logo.ScaleTo(5, 1000, Easing.CubicOut),
                logo.RotateYTo(360, 1000, Easing.CubicOut),
                logo.FadeTo(0, 700, Easing.CubicOut)
            );
            await albumArt.FadeTo(1);
        }
    }
}
