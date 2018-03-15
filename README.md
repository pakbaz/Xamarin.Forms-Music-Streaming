# Xamarin.Forms Music Streaming Time Based Custom Metadata With .NET 2.0 Support
Xamarin Forms Music Streaming Online Radio Station App with Custom Metadata and Background Music Playback. Support SHOUTCast and IceCast and custom time based dynamic metadata and album art.

## Background...
After Creating my online radio I thought about creating beautiful Cross platfomr native app using Xamarin.Forms with some basic features like playing and stopping and updating time based metadata that reads from the server and I thought to myself this should be easy but In fact It was not. not only there is no out of the box media player support in Xamarin.Forms but also even inside each platform in my case iOS and Android there are so many implementation for mediaplayer specially in android There is a lot of ways to implement background audio for instance and documentation is very confusing to say the least. There is no straight forward guide in neither iOS or android documentation for displaying background audio information and changing metadata for example.

## Media Manager Plugin 
There is a very nice cross [platform plugin](https://github.com/martijn00/XamarinMediaManager) for Xamarin.Forms by [Martijn van Dijk](https://github.com/martijn00) which is very easy to use and has a great compatibility and features. but there were several problems with it that I couldn't use it
1. It doesn't support .NET Standard 2.0 yet though it is been worked on for future release
2. It lacks certain customizations specially around Lockscreen remote media player on iOS (disabling previous/next track for example track control and instead displaying Live icon and even displaying album art was not possible)
3. It could not handle network interruptions the way I wanted to
4. Pause On remote screen was causing me issues sometime because I was playing livestream, pause and play were resulting into the stream failing to resume properly. Instead I wanted Pause action to perform stop and play will resume from the current live rasio instead of resuming the stream from memory.

## My Code
Eventhough, Media Manager plugin is very nice and works wonderful for many scenarios, I quickly realized It was not made for me and I challenged myself into creating a custom native implementation that suits my needs. I created an Interface in my .NET Standard 2.0 class library and used dependency Injection service to implement them seperately for iOS and Android.

Here are few keypoint to help you navigate the code:
- I created a simple Inteface called IAudioPlayer in Standard class library which I have implemented in iOS and Android
- Background audio playmode is enabled and its done using AVPlayer in iOS and Background Service in Android
- Custom Time based Metadata information is retrieved from URL and saved into a custom model (LiveInfo) and a timer is used to update both app and background audio metadata information regularly. I currently use Radio.Co for my online radio but It can be modified to use SHOUTcast or any ICEcast or similar radio streaming services.

## Future Enhancement
- Retrieving live metadata information could be changed from constant pulling data to observable pattern for better result and performance. I just didn't have time to pursue that
- In android, I didn't need to support older devices but I could use AppCompat library to support back to V7 or V4 
- Some android classes I used are going to be obsolete in future version which could be refreshed
- Better method to identify player Buffering or perhaps even better implementation of player status code similar to Media Manager Plugin
