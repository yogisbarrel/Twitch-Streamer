using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
<<<<<<< HEAD
using Windows.Web.Http;
using TwitchStreamer.Objects;
using TwitchStreamer.Objects.Channel;
=======
using System.Linq;
>>>>>>> origin/master

namespace TwitchStreamer.Views
{
    public sealed partial class Channels : Page
    {
        private string tokenAPI = "http://api.twitch.tv/api/channels/{0}/access_token";
        public string gameTemp;
        public string m3URL = "http://usher.twitch.tv/api/channel/hls/{0}.m3u8?player=twitchweb&token={1}&sig={2}&allow_audio_only=true&allow_source=true&type=any";
        private List<ChanStrim> strims = new List<ChanStrim>();
        private RootObject root = new RootObject();
        private AccessToken toke = new AccessToken();
        private List<ChanStrim> chanLs = new List<ChanStrim>();

<<<<<<< HEAD
        /// <summary>
        /// Initializes a new instance of the <see cref="Channels"/> class.
        /// </summary>
=======
>>>>>>> origin/master
        public Channels()
        {
            this.InitializeComponent();
        }

<<<<<<< HEAD
        /// <summary>
        /// Strims the view.
        /// </summary>
        /// <param name="temp">The temporary.</param>
=======
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
>>>>>>> origin/master
        public void strimView(List<ChanStrim> temp)
        {
            streamView.ItemsSource = temp;
        }

<<<<<<< HEAD
        /// <summary>
        /// Runs the stream tiles.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
=======
>>>>>>> origin/master
        async public Task<RootObject> runStreamTiles(string game)
        {
            Uri api = new Uri("https://api.twitch.tv/kraken/streams?game=" + game);
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

<<<<<<< HEAD
        /// <summary>
        /// Converts the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
=======
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

>>>>>>> origin/master
        private ImageBrush convert(string link)
        {
            try
            {
                ImageBrush img = new ImageBrush();
                var source = new BitmapImage();
                source.UriSource = new Uri(link);
                img.ImageSource = source;

                return img;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Page_s the loading.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        async private void Page_Loading(FrameworkElement sender, object args)
        {
            RootObject chan = await runStreamTiles(gameTemp);
<<<<<<< HEAD
            foreach (var tep in chan.streams)
            {
                var t = new ChanStrim
                {
                    name = tep.channel.name,
                    displayName = tep.channel.display_name,
                    viewers = tep.viewers,
                    preview = convert(tep.preview.medium).ImageSource,
                };
                chanLs.Add(t);
            }

=======

            //populateChannels(chan);
            //streamView.UpdateLayout();
>>>>>>> origin/master
            streamView.ItemsSource = chanLs;
            streamView.UpdateLayout();
            var item = (from p in AppShell.Current.navlist where p.DestinationPage == typeof(Channels) select p).SingleOrDefault();

            var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

            if (container != null) container.IsTabStop = false;
            AppShell.Current.NavMenuList.SetSelectedItem(container);
            if (container != null) container.IsTabStop = true;
        }

<<<<<<< HEAD
        /// <summary>
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            gameTemp = e.Parameter.ToString();
=======
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
>>>>>>> origin/master
        }

        /// <summary>
        /// Handles the ItemClick event of the streamView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void streamView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (ChanStrim)e.ClickedItem;
<<<<<<< HEAD
=======
            
            AppShell.Current.AppFrame.Navigate(typeof(NowPlaying), item.m3uLink);
>>>>>>> origin/master
        }

        /// <summary>
        /// Called when [navigating to page].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NavigatingCancelEventArgs"/> instance containing the event data.</param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item = (from p in AppShell.Current.navlist where p.DestinationPage == e.SourcePageType select p).SingleOrDefault();
                if (item == null && AppShell.Current.AppFrame.BackStackDepth > 0)
                {
                    foreach (var entry in AppShell.Current.AppFrame.BackStack.Reverse())
                    {
                        item = (from p in AppShell.Current.navlist where p.DestinationPage == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                            break;
                    }
                }

                var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

                if (container != null) container.IsTabStop = false;
                AppShell.Current.NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }
    }

<<<<<<< HEAD
    
}
=======
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
>>>>>>> origin/master
