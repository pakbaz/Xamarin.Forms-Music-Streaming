using Radio021.Droid;
using Radio021.Interfaces;
using System.Threading.Tasks;
using Android.Content;
using Xamarin.Forms;
using Android.App;
using Android.Graphics;
using System.Net;
using System;
using Android.Media.Session;

[assembly: Dependency(typeof(AudioPlayer))]
namespace Radio021.Droid
{
    public class AudioPlayer : IAudioPlayer
    {
        NotificationManager notificationManager;

        public AudioPlayer()
        {
            notificationManager = NotificationManager.FromContext(Forms.Context);
        }
        public bool IsPlaying => StreamingBackgroundService.player?.IsPlaying == true;

        public bool IsBuffering => StreamingBackgroundService.isBuffering;

        public void Pause()
        {

            SendAudioCommand(StreamingBackgroundService.ActionPause);
        }

        public async Task Play()
        {
            SendAudioCommand(StreamingBackgroundService.ActionPlay);
        }

        public void Stop()
        {
            SendAudioCommand(StreamingBackgroundService.ActionStop);
        }

        public async Task Restart()
        {
            Stop();
            await Play();

        }

        public void SetMetaData(string title, string artist, string album, string albumArtUrl)
        {

            var notification = new Notification.Builder(Forms.Context)
                                           .SetSmallIcon(Resource.Drawable.logoSmall)
                                           .SetContentTitle(title)
                                           .SetContentText(artist)
                                           .SetContentInfo(album)
                                           .SetLargeIcon(GetBitmapFromUrl(albumArtUrl))
                                           .SetStyle(new Notification.MediaStyle())
                                           .SetVisibility(NotificationVisibility.Public)
                                           .AddAction(GenerateAction(Android.Resource.Drawable.IcMediaPlay, "Play", StreamingBackgroundService.ActionPlay))
                                           .AddAction(GenerateAction(Android.Resource.Drawable.IcMediaPause, "Pause", StreamingBackgroundService.ActionStop))
                                           .Build();
            
            notificationManager.Notify(1, notification);
            
        }

        private static Notification.Action GenerateAction(int icon, String title, String intentAction)
        {
            Intent intent = new Intent(Forms.Context, typeof(StreamingBackgroundService));
            intent.SetAction(intentAction);

            PendingIntentFlags flags = PendingIntentFlags.UpdateCurrent;
            if (intentAction.Equals(StreamingBackgroundService.ActionStop))
                flags = PendingIntentFlags.CancelCurrent;

            PendingIntent pendingIntent = PendingIntent.GetService(Forms.Context, 1, intent, flags);

            return new Notification.Action.Builder(icon, title, pendingIntent).Build();
        }

        public static Bitmap GetBitmapFromUrl(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] bytes = webClient.DownloadData(url);
                if (bytes != null && bytes.Length > 0)
                {
                    return BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                }
            }
            return null;
        }


        private void SendAudioCommand(string action)
        {
            var intent = new Intent(action);
            Forms.Context.StartService(intent);
        }


    }


}
