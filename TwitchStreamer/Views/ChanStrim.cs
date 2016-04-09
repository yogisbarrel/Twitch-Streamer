using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace TwitchStreamer.Views
{
    public class ChanStrim
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public ImageSource preview { get; set; }
        public int viewers { get; set; }
        public Uri m3uLink { get; set; }
    }


    
}
