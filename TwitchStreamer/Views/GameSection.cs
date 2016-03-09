using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace TwitchStreamer.Views
{
    public class GameSection
    {
        public string game { get; set; }
        public ImageSource gameImage { get; set; }
        public int viewers { get; set; }
        public Type DestinationPage { get; set; }
    }
}
