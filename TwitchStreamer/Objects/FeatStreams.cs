﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace TwitchStreamer.Objects
{
    public class FeatStreams
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public ImageSource preview { get; set; }
        public int viewers { get; set; }
        public RootObject root { get; set; }
    }
    public class Links
    {
        public string self { get; set; }
        public string next { get; set; }
    }

    public class Preview
    {
        public string small { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string template { get; set; }
    }

    public class Links2
    {
        public string self { get; set; }
    }

    public class Links3
    {
        public string self { get; set; }
        public string follows { get; set; }
        public string commercial { get; set; }
        public string stream_key { get; set; }
        public string chat { get; set; }
        public string features { get; set; }
        public string subscriptions { get; set; }
        public string editors { get; set; }
        public string teams { get; set; }
        public string videos { get; set; }
    }

    public class Channel
    {
        public bool? mature { get; set; }
        public string status { get; set; }
        public string broadcaster_language { get; set; }
        public string display_name { get; set; }
        public string game { get; set; }
        public string language { get; set; }
        public int _id { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object delay { get; set; }
        public string logo { get; set; }
        public object banner { get; set; }
        public string video_banner { get; set; }
        public object background { get; set; }
        public string profile_banner { get; set; }
        public string profile_banner_background_color { get; set; }
        public bool partner { get; set; }
        public string url { get; set; }
        public int views { get; set; }
        public int followers { get; set; }
        public Links3 _links { get; set; }
    }

    public class Stream
    {
        public object _id { get; set; }
        public string game { get; set; }
        public int viewers { get; set; }
        public int video_height { get; set; }
        public double average_fps { get; set; }
        public int delay { get; set; }
        public string created_at { get; set; }
        public bool is_playlist { get; set; }
        public Preview preview { get; set; }
        public Links2 _links { get; set; }
        public Channel channel { get; set; }
    }

    public class Featured
    {
        public string text { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public bool sponsored { get; set; }
        public int priority { get; set; }
        public bool scheduled { get; set; }
        public Stream stream { get; set; }
    }

    public class RootObject
    {
        public Links _links { get; set; }
        public List<Featured> featured { get; set; }
    }
}