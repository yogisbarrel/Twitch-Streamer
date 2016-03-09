using System;
using Windows.UI.Xaml.Controls;
using TwitchStreamer.Objects;
using Windows.Web.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;
using Windows.UI.Xaml.Navigation;

namespace TwitchStreamer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Featured : Page
    {
        bool load = true;
        FeatStreams feat = new FeatStreams();
        List<FeatStreams> ftr = new List<FeatStreams>();
        public Featured()
        {
            this.InitializeComponent();

            this.Loading += (sender, args) =>
            {
                runStreamTiles();
            };
            this.Loaded += (sender, args) =>
            {
                
                //runStreamTiles();
                if (load == true)
                    featuredView.Visibility = Windows.UI.Xaml.Visibility.Visible;
                else
                    featuredView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                var item = (from p in AppShell.Current.navlist where p.DestinationPage == typeof(Featured) select p).SingleOrDefault();

                var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                AppShell.Current.NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            };
        }

        async public void runStreamTiles()
        {
            Uri api = new Uri("https://api.twitch.tv/kraken/streams/featured");
            string test = string.Empty;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(api);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode == false)
                    load = false;
                else if (response.IsSuccessStatusCode == true)
                    load = true;
                test = await response.Content.ReadAsStringAsync();
            }
            feat.root = JsonConvert.DeserializeObject<Objects.RootObject>(test);

                //= await runStreamTiles(gameTemp);
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
            //streamView.UpdateLayout();
            featuredView.ItemsSource = ftr;
            featuredView.UpdateLayout();
            
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

        private void featuredView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (FeatStreams)e.ClickedItem;
            
            AppShell.Current.AppFrame.Navigate(typeof(NowPlaying), item.name);
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
}
