using System;
using System.Threading.Tasks;

namespace Radio021.Interfaces
{
    public interface IAudioPlayer
    {
        bool IsPlaying { get; }

        bool IsBuffering { get;}

        Task Play();

		Task Restart();
  
        void Pause();

        void Stop();

        void SetMetaData(string title, string artist, string album, string albumArtUrl);
        
    }
}
