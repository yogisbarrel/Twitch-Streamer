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
using Windows.Web.Http;
using TwitchStreamer.Objects;
using TwitchStreamer.Objects.Channel;

namespace TwitchStreamer.Views
{
    public sealed partial class Channels : Page
    {
        public string gameTemp;
        private List<ChanStrim> strims = new List<ChanStrim>();
        private RootObject root = new RootObject();
        private List<ChanStrim> chanLs = new List<ChanStrim>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Channels"/> class.
        /// </summary>
        public Channels()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Strims the view.
        /// </summary>
        /// <param name="temp">The temporary.</param>
        public void strimView(List<ChanStrim> temp)
        {
            streamView.ItemsSource = temp;
        }

        /// <summary>
        /// Runs the stream tiles.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        async public Task<RootObject> runStreamTiles(string game)
        {
            Uri api = new Uri("https://api.twitch.tv/kraken/streams?game=" + game);
            string test;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(api);
                response.EnsureSuccessStatusCode();
                test = await response.Content.ReadAsStringAsync();
            }
            root = JsonConvert.DeserializeObject<RootObject>(test);

            return root;
        }

        /// <summary>
        /// Converts the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
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

            streamView.ItemsSource = chanLs;
            streamView.UpdateLayout();
            var item = (from p in AppShell.Current.navlist where p.DestinationPage == typeof(Channels) select p).SingleOrDefault();

            var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

            if (container != null) container.IsTabStop = false;
            AppShell.Current.NavMenuList.SetSelectedItem(container);
            if (container != null) container.IsTabStop = true;
        }

        /// <summary>
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            gameTemp = e.Parameter.ToString();
        }

        /// <summary>
        /// Handles the ItemClick event of the streamView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void streamView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (ChanStrim)e.ClickedItem;
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

    
}