using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;

namespace Radio021.Droid
{
    [Activity(Label = "Radio021", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());

            App.ScreenWidth = Resources.DisplayMetrics.WidthPixels; // real pixels
            App.ScreenHeight = Resources.DisplayMetrics.HeightPixels; // real pixels

        }

        protected override void OnDestroy()
        {
            var notificationManager = NotificationManager.FromContext(Forms.Context);


            notificationManager.CancelAll();
            notificationManager.Dispose();
            base.OnDestroy();
        }

    }
}

