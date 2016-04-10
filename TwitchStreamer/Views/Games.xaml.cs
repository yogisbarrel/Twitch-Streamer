using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchStreamer.Objects;
using TwitchStreamer.Objects.Game;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

namespace TwitchStreamer.Views
{
    /// <summary>
    /// The page that displays the current trending games as tiles
    /// live from twitch.tv
    /// </summary>
    public sealed partial class Games : Page
    {
        static Uri api = new Uri("https://api.twitch.tv/kraken/games/top?limit=50");

        private List<GameSection> gridList = new List<GameSection>();
        private Objects.Game.RootObject GamesInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Games"/> class.
        /// </summary>
        public Games()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Games the tiles.
        /// </summary>
        private void gameTiles()
        {
            var f = GamesInfo.top.Select(item => new GameSection()
            {
                game = item.game.name,
                viewers = item.viewers,
                gameImage = convert(item.game.box.large).ImageSource,
            }).ToList();
            gridList.AddRange(f);
            gameView.ItemsSource = gridList;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <returns></returns>
        async public Task<Objects.Game.RootObject> GetAsync()
        {
            string test;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(api);
                response.EnsureSuccessStatusCode();
                test = await response.Content.ReadAsStringAsync();
            }

            GamesInfo = JsonConvert.DeserializeObject<Objects.Game.RootObject>(test);

            return GamesInfo;
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
        private async void Page_Loading(FrameworkElement sender, object args)
        {
            GamesInfo = await GetAsync();
            gameTiles();
        }

        /// <summary>
        /// Handles the ItemClick event of the gameView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void gameView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (GameSection)e.ClickedItem;
            AppShell.Current.AppFrame.Navigate(typeof(Channels), item.game);
        }

        /// <summary>
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
            }
            gameView.IsEnabled = true;
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

        /// <summary>
        /// Raises the <see cref="E:NavigatedFrom" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            gameView.IsEnabled = false;
        }
    }
}