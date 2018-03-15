using System;
using System.Collections.Generic;

namespace Radio021.Models
{
    public class Source
    {
        public string type { get; set; }
        public object collaborator { get; set; }
        public object relay { get; set; }
    }

    public class CurrentTrack
    {
        public string title { get; set; }
        public DateTime start_time { get; set; }
        public string artwork_url { get; set; }
        public string artwork_url_large { get; set; }
    }

    public class History
    {
        public string title { get; set; }
        public DateTime start_time { get; set; }
        public string artwork_url { get; set; }
    }

    public class Output
    {
        public string name { get; set; }
        public string format { get; set; }
        public int bitrate { get; set; }
    }

    public class LiveInfo
    {
        public string status { get; set; }
        public Source source { get; set; }
        public CurrentTrack current_track { get; set; }
        public List<History> history { get; set; }
        public string logo_url { get; set; }
        public string streaming_hostname { get; set; }
        public List<Output> outputs { get; set; }
    }

    public class HistoryTracks
    {
        public List<History> tracks { get; set; }
    }
}
