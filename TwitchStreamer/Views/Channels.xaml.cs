using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
using TwitchStreamers.Views;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Newtonsoft.Json;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Resources;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace TwitchStreamer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Channels : Page
    {
        private string tokenAPI = "http://api.twitch.tv/api/channels/{0}/access_token";
        public string gameTemp;
        public string m3URL = "http://usher.twitch.tv/api/channel/hls/{0}.m3u8?player=twitchweb&token={1}&sig={2}&allow_audio_only=true&allow_source=true&type=any";
        private List<ChanStrim> strims = new List<ChanStrim>();
        private RootObject root = new RootObject();
        private AccessToken toke = new AccessToken();
        private List<ChanStrim> chanLs = new List<ChanStrim>();

        public Channels()
        {
            this.InitializeComponent();
        }

        public async Task<Uri> getm3URL(string channel)
        {
            Uri token = new Uri(string.Format(tokenAPI, channel));
            string test;
            test = await webRequest(token);
            toke = JsonConvert.DeserializeObject<AccessToken>(test);
            string[] args = {channel, toke.token, toke.sig};
            string.Format(m3URL, args);
            return new Uri(m3URL);
        }
        public void strimView(List<ChanStrim> temp)
        {
            streamView.ItemsSource = temp;
            //streamView.UpdateLayout();
        }

        async public Task<RootObject> runStreamTiles(string game)
        {
            Uri api = new Uri("https://api.twitch.tv/kraken/streams?game="+game);
            string test;
            //test = await webRequest(api);
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(api);
                response.EnsureSuccessStatusCode();
                test = await response.Content.ReadAsStringAsync();
            }
            root = JsonConvert.DeserializeObject<RootObject>(test);

            return root;
        }

        async public Task<string> webRequest(Uri conn)
        {
            string test;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(conn);
                response.EnsureSuccessStatusCode();
                test = await response.Content.ReadAsStringAsync();
            }
            return test;
        }

        private ImageBrush convert(string link)
        {
            try
            {
                ImageBrush img = new ImageBrush();
                var source = new BitmapImage();
                source.UriSource = new Uri(link);
                img.ImageSource = source;

                return img;
                //return img;
            }
            catch (NullReferenceException)
            {
                //MessageBox.Show(e.Message);
                return null;
            }
        }

        async private void Page_Loading(FrameworkElement sender, object args)
        {
            //gameTemp = e.Parameter.ToString();

            RootObject chan = await runStreamTiles(gameTemp);

            //populateChannels(chan);
            //streamView.UpdateLayout();
            streamView.ItemsSource = chanLs;
            streamView.UpdateLayout();
            var item = (from p in AppShell.Current.navlist where p.DestinationPage == typeof(Channels) select p).SingleOrDefault();

            var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

            // While updating the selection state of the item prevent it from taking keyboard focus.  If a
            // user is invoking the back button via the keyboard causing the selected nav menu item to change
            // then focus will remain on the back button.
            if (container != null) container.IsTabStop = false;
            AppShell.Current.NavMenuList.SetSelectedItem(container);
            if (container != null) container.IsTabStop = true;
        }

        async private void populateChannels(RootObject channels)
        {
            foreach(var channel in channels.streams)
            {
                var tile = new ChanStrim
                {
                    name = channel.channel.name,
                    displayName = channel.channel.display_name,
                    viewers = channel.viewers,
                    preview = convert(channel.preview.medium).ImageSource,
                    m3uLink = await getm3URL(channel.channel.name)
                };
                chanLs.Add(tile);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //((Page)sender).Focus(FocusState.Programmatic);
            //((Page)sender).Loaded -= Page_Loaded;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            gameTemp = e.Parameter.ToString();           
        }

        private void streamView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (ChanStrim)e.ClickedItem;
            
            AppShell.Current.AppFrame.Navigate(typeof(NowPlaying), item.m3uLink);
        }

        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item = (from p in AppShell.Current.navlist where p.DestinationPage == e.SourcePageType select p).SingleOrDefault();
                if (item == null && AppShell.Current.AppFrame.BackStackDepth > 0)
                {
                    // In cases where a page drills into sub-pages then we'll highlight the most recent
                    // navigation menu item that appears in the BackStack
                    foreach (var entry in AppShell.Current.AppFrame.BackStack.Reverse())
                    {
                        item = (from p in AppShell.Current.navlist where p.DestinationPage == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                            break;
                    }
                }

                var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                AppShell.Current.NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }
    }
    public class Links
    {
        public string self { get; set; }
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
        public Links2 _links { get; set; }
    }

    public class Stream
    {
        public object _id { get; set; }
        public string game { get; set; }
        public int viewers { get; set; }
        public string created_at { get; set; }
        public int video_height { get; set; }
        public double average_fps { get; set; }
        public int delay { get; set; }
        public bool is_playlist { get; set; }
        public Links _links { get; set; }
        public Preview preview { get; set; }
        public Channel channel { get; set; }
    }

    public class Links3
    {
        public string self { get; set; }
        public string next { get; set; }
        public string featured { get; set; }
        public string summary { get; set; }
        public string followed { get; set; }
    }

    public class RootObject
    {
        public List<Stream> streams { get; set; }
        public int _total { get; set; }
        public Dictionary<string, string> _links { get; set; }        
    }

    public class AccessToken
    {
        public string token { get; set; }
        public string sig { get; set; }
        public bool mobile_restricted { get; set; }
    }
}
