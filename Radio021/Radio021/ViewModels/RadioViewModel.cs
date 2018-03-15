using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Radio021.ViewModels
{
    public class RadioViewModel : BaseViewModel
    {
        private string _image;
        private string _info;
        private bool _timerIsSet;

        public RadioViewModel()
        {
            _image = "stop";

            MessagingCenter.Subscribe<App>(this, "playback", (sender) =>
            {
                SetInfo();
                UpdateButton();
            });


            MessagingCenter.Subscribe<App>(this, "metadata", (sender) =>
            {
                SetInfo();
                IsBusy = false;
            });

            if (App._metadata == null)
            {
                IsBusy = true;
            }

            SetInfo();
            UpdateButton();

            if (!_timerIsSet)
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    SetInfo();
                    return true;
                });
                _timerIsSet = true;
            }
        }

        private void UpdateButton()
        {
            if (App._AudioPlayer.IsPlaying)
            {
                ButtonImage = "stop";
            }
            else
            {
                ButtonImage = "play";
            }
        }

        private void SetInfo()
        {
            if (App._metadata != null)
            {
                if (App._metadata.source.type == "automated")
                {
                    if (App._AudioPlayer.IsPlaying)
                    {
                        SongInfo = string.Format("Playing: {0} ({1}) {2}{3}",
                                               App._metadata.source.type == "automated" ? "DJ" : App._metadata.source.type,
                                               (DateTime.Now - App._metadata.current_track.start_time).ToString("mm':'ss"),
                                                Environment.NewLine,
                                                App._metadata.current_track.title);
                    }
                    else
                    {
                        if (App._AudioPlayer.IsBuffering)
                            SongInfo = " Buffering...";
                        else
                            SongInfo = " Stopped";
                    }
                }
                else
                {
                    SongInfo = " Playing Live Show";

                }
            }
            else
            {
                SongInfo = " Radio 021";
            }


        }

        public string ButtonImage
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public string SongInfo
        {
            get { return _info; }
            set { SetProperty(ref _info, value); }
        }

        public ICommand PlayStopCommand => new Command(() =>
        {
            if (App._AudioPlayer.IsPlaying)
            {
                App._AudioPlayer.Stop();
                ButtonImage = "play";
            }
            else
            {
                App._AudioPlayer.Play();
                ButtonImage = "stop";
            }
            SetInfo();
        });
    }
}
