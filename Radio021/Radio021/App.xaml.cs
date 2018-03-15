using System;
using System.Net.Http;
using System.Threading.Tasks;
using Radio021.Models;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Radio021.Interfaces;

namespace Radio021
{
    public partial class App : Application
    {
        public const string RadioName = "Radio 021";
        public const string Stream_URL = "YOUR STREAM URL";
        public const string INFO_URL = "INFO URL";

        public static int ScreenWidth;
        public static int ScreenHeight;
        public static bool IsPlaying;

        public static LiveInfo _metadata;
        public static bool _metadataLoaded;
        public HttpClient _client;

        public static IAudioPlayer _AudioPlayer;

        public App()
        {
            InitializeComponent();
            _AudioPlayer = DependencyService.Get<IAudioPlayer>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start("ios=youriosappCenterCode;" + "android=yourandroidappCentercode;", typeof(Analytics), typeof(Crashes));
            //SetMetaData();

            _AudioPlayer.Play();

            BackgroundProcessing();

        }




        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            if (!_AudioPlayer.IsPlaying)
            {
                _AudioPlayer.Restart();
            }
        }


        void BackgroundProcessing()
        {
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
             {
                 try
                 {
                     Task.Factory.StartNew(async () => {
                         var meta = await RefreshDataAsync();
                         if (_metadata == null ||
                                     _metadata.current_track.title != meta.current_track.title ||
                                     _metadata.source.type != meta.source.type)
                         {
                             _metadata = meta;
                             await UpdateHistory();

                             _metadataLoaded = true;
                             SetMetaData();

                         }
                         SetPlayback();

                     });
                     return true;
                 }
                 catch (Exception)
                 {
                     // we still want to continue processing backgroud task even when there is an error
                     return true;
                 }
               
             });
           


        }

        private void SetPlayback()
        {
            if (_AudioPlayer.IsPlaying != IsPlaying)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send(this, "playback");
                });
                IsPlaying = _AudioPlayer.IsPlaying;
            }
        }

        void SetMetaData()
        {
            string track = _metadata.current_track.title;

            var artist = track.Substring(0, track.IndexOf('-'))?.Trim();
            var title = track.Substring(track.IndexOf('-')+1)?.Trim();
            var album = "Radio 021";
            _AudioPlayer.SetMetaData(title,artist,album,_metadata.current_track.artwork_url_large);

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send(this, "metadata");
            });
        }


        async Task<LiveInfo> RefreshDataAsync()
        {
            _client = new HttpClient();
            var uri = new Uri(INFO_URL + "status");
            var response = await _client.GetStringAsync(uri);
            LiveInfo info = new LiveInfo();
            info = JsonConvert.DeserializeObject<LiveInfo>(response);

            return info;
        }
        async Task UpdateHistory()
        {
            _client = new HttpClient();
            var uri = new Uri(INFO_URL + "history");
            var response = await _client.GetStringAsync(uri);
            HistoryTracks info = new HistoryTracks();
            info = JsonConvert.DeserializeObject<HistoryTracks>(response);

            _metadata.history = info.tracks;
        }


    }
}
