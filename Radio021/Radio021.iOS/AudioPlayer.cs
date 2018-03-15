using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVFoundation;
using CoreMedia;
using Foundation;
using MediaPlayer;
using Radio021.Interfaces;
using Radio021.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayer))]
namespace Radio021.iOS
{
    public class AudioPlayer : IAudioPlayer
    {
        protected string FileName = App.Stream_URL;
        protected AVPlayer _player;
        private AVPlayerItem _playerItem;
        private bool _buffering;

        public AudioPlayer()
        {
            AVAudioSession audioSession = AVAudioSession.SharedInstance();
            NSError error;
            audioSession.SetCategory(AVAudioSession.CategoryPlayback, out error);
            audioSession.SetActive(true, out error);



            UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();

            var commandCenter = MPRemoteCommandCenter.Shared;
            commandCenter.TogglePlayPauseCommand.Enabled = true;
            commandCenter.TogglePlayPauseCommand.AddTarget(TogglePlayPauseCommand);


        }

        public bool IsPlaying { get { return _player?.TimeControlStatus == AVPlayerTimeControlStatus.Playing; } }
        public bool IsBuffering { get { return (_playerItem?.PlaybackBufferFull != true && _buffering); } }

        public async Task Play()
        {
            _buffering = true;
            using (var url = NSUrl.FromString(FileName))
            {
                if (_player == null)
                {
                    _playerItem = new AVPlayerItem(url);
                    _player = AVPlayer.FromPlayerItem(_playerItem);
                }
            }

            SetupCriticalNotifications();

            do
            {
                await Task.Delay(5);
            } while (_player.Status != AVPlayerStatus.ReadyToPlay);

            _player.Play();

            _player.AutomaticallyWaitsToMinimizeStalling = true;

        }

        private void SetupCriticalNotifications()
        {
            NSNotificationCenter.DefaultCenter.RemoveObservers(new List<NSObject>(){
                        AVPlayerItem.NewErrorLogEntryNotification,
                        AVPlayerItem.PlaybackStalledNotification,
                        AVPlayerItem.ItemFailedToPlayToEndTimeNotification
                    });
            NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.NewErrorLogEntryNotification, OnNetworkInterruption, _playerItem);
            NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.PlaybackStalledNotification, OnNetworkInterruption, _playerItem);
            NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.ItemFailedToPlayToEndTimeNotification, OnNetworkInterruption, _playerItem);
        }

        public void Pause()
        {
            _buffering = false;
            _player.Pause();
        }

        public void Stop()
        {
            _buffering = false;
            if (_player == null) return;
            _player.Dispose();
            _player = null;
        }

        public bool HasFile
        {
            get { return _player != null; }
        }


        private void OnNetworkInterruption(NSNotification obj)
        {
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                if (IsPlaying)
                {
                    return false;
                }

                Restart();
                return true;
                    
            });

        }

        public async Task  Restart()
        {
            Stop();
            await Play();
        }

        private MPRemoteCommandHandlerStatus TogglePlayPauseCommand(MPRemoteCommandEvent arg)
        {
            if (IsPlaying)
            {
                Stop();
            }
            else
            {
                Play();
            }

            return MPRemoteCommandHandlerStatus.Success;
        }

        public void SetMetaData(string title, string artist, string album, string albumArtUrl)
        {
            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = new MPNowPlayingInfo()
            {
                Title = title,
                Artist = artist,
                AlbumTitle = album,
                Artwork = new MPMediaItemArtwork(FromUrl(albumArtUrl)),
                IsLiveStream = true
            };

        }

        static UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }
    }

   
}
