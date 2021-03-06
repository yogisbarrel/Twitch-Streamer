﻿using Windows.UI.Xaml.Media;

namespace TwitchStreamer.Objects
{
    public class Playing
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public ImageSource preview { get; set; }
        public int viewers { get; set; }
    }
}