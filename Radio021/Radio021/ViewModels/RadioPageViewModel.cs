using System.Collections.Generic;
using System.Threading.Tasks;
using Radio021.Models;
using Xamarin.Forms;

namespace Radio021.ViewModels
{
    public class RadioPageViewModel : BaseViewModel
    {

        private List<History> _history;
        private string _artUrl;
        private string _currentSong;

        public RadioPageViewModel()
        {
            if (App._metadataLoaded)
            {
                SetMetaDataInfo();
                IsBusy = false;
            }
            else
            {
                IsBusy = true;
            }

            MessagingCenter.Subscribe<App>(this, "metadata", (obj) =>
            {
                SetMetaDataInfo();
                if (IsBusy)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(1000);
                        IsBusy = false;
                    });
                }
               

            });
        }

        private void SetMetaDataInfo()
        {
            if (App._metadata.source.type == "automated")
            {
                CurrentSong = App._metadata.current_track.title;
                ArtUrl = App._metadata?.current_track.artwork_url_large;
            }
            else
            {
                CurrentSong = "Live Show";
                ArtUrl = App._metadata.logo_url;
            }

            History = App._metadata?.history;

        }

        public List<History> History
        {
            get { return _history; }
            set { SetProperty(ref _history, value); }
        }

        public string CurrentSong
        {
            get { return _currentSong; }
            set { SetProperty(ref _currentSong, value); }
        }
        public string ArtUrl
        {
            get { return _artUrl; }
            set { SetProperty(ref _artUrl, value); }
        }
    }
}
