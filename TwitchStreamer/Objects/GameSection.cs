using System;
using Windows.UI.Xaml.Media;

namespace TwitchStreamer.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public class GameSection
    {
        public string game { get; set; }
        public ImageSource gameImage { get; set; }
        public int viewers { get; set; }
        public Type DestinationPage { get; set; }
    }    
}