using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchStreamer.Objects;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

namespace TwitchStreamer.Views
{
    public sealed partial class Featured : Page
    {
        private bool load = true;
        private FeatStreams feat = new FeatStreams();
        private List<FeatStreams> ftr = new List<FeatStreams>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Featured"/> class.
        /// </summary>
        public Featured()
        {
            this.InitializeComponent();

            this.Loading += (sender, args) =>
            {
                runStreamTiles();                
            };
            this.Loaded += (sender, args) =>
            {
                if (load == true)
                    featuredView.Visibility = Windows.UI.Xaml.Visibility.Visible;
                else
                    featuredView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                var item = (from p in AppShell.Current.navlist where p.DestinationPage == typeof(Featured) select p).SingleOrDefault();

                var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

                if (container != null) container.IsTabStop = false;
                AppShell.Current.NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            };
        }

        /// <summary>
        /// Runs the stream tiles.
        /// </summary>
        public async void runStreamTiles()
        {
            Uri api = new Uri("https://api.twitch.tv/kraken/streams/featured");
            string jsonObject = string.Empty;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(api);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode == false)
                    load = false;
                else if (response.IsSuccessStatusCode == true)
                    load = true;
                jsonObject = await response.Content.ReadAsStringAsync();
            }

            feat.root = JsonConvert.DeserializeObject<Objects.Featured.RootObject>(jsonObject);            
        }

        /// <summary>
        /// Populates the tiles.
        /// </summary>
        public void populateTiles()
        {
            foreach (var tep in feat.root.featured)
            {
                var t = new FeatStreams
                {
                    name = tep.stream.channel.name,
                    displayName = tep.stream.channel.display_name,
                    viewers = tep.stream.viewers,
                    preview = convert(tep.stream.preview.medium).ImageSource,
                };
                ftr.Add(t);
            }
            featuredView.ItemsSource = ftr;
            featuredView.UpdateLayout();
        }
        /// <summary>
        /// Retrieves and image from the specified URL.
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
        /// Handles the ItemClick event of the featuredView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void featuredView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (FeatStreams)e.ClickedItem;

            AppShell.Current.AppFrame.Navigate(typeof(NowPlaying), item.name);
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