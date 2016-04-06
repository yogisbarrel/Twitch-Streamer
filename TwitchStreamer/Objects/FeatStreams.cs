using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using TwitchStreamer.Objects.Featured;
namespace TwitchStreamer.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public class FeatStreams
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public ImageSource preview { get; set; }
        public int viewers { get; set; }
        public RootObject root { get; set; }
    }    
}