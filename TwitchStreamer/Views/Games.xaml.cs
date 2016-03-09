using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using TwitchStreamers.Views;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Newtonsoft.Json;
using TwitchStreamer;
using System.Linq;

namespace TwitchStreamer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Games : Page
    {
        private List<GameSection> gridList = new List<GameSection>();
        //public static AppShell Current = null;
        public string game;
        Uri api = new Uri("https://api.twitch.tv/kraken/games/top?limit=50");
        private GamesInfo.RootObject deser;
        private List<ChanStrim> chanLs = new List<ChanStrim>();
        public Games()
        {
            //parseGames();
            //parseGames()
            this.InitializeComponent();      

            
        }
        private void gameTiles()
        {
            
           
            var f = deser.top.Select(item => new GameSection()
            {
                game = item.game.name,
                viewers = item.viewers,
                gameImage = convert(item.game.box.large).ImageSource,
            }).ToList();
            gridList.AddRange(f);
            gameView.ItemsSource = gridList;
           
        }
        //private async Task parseGames()
        //{
        //    //temp = new GamesInfo();
        //    //pooser = await temp.RunAsync();
        //    //var f = gameTiles();
            
        //    Action glist = () => gameTiles();
        //    await Task.Run(glist);           
            
        //}

        async public Task<GamesInfo.RootObject> GetAsync()
        {
            string test;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(api);
                response.EnsureSuccessStatusCode();
                test = await response.Content.ReadAsStringAsync();
            }

            deser = JsonConvert.DeserializeObject<GamesInfo.RootObject>(test);


            return deser;
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

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            deser = await GetAsync();
            gameTiles();
            
            //gameView.UpdateLayout();
        }

        private void gameView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (GameSection)e.ClickedItem;
            AppShell.Current.AppFrame.Navigate(typeof(Channels), item.game);
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                
            }
            gameView.IsEnabled = true;
        }
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
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            //this.GamePage.Frame.Dispatcher.
            gameView.IsEnabled = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
