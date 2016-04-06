using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TwitchStreamer.Views
{
    public sealed partial class NowPlaying : Page
    {
        private string iframe = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="NowPlaying"/> class.
        /// </summary>
        public NowPlaying()
        {
            this.InitializeComponent();
           
            this.Loaded += (sender, args) =>
            {
                var item = (from p in AppShell.Current.navlist where p.DestinationPage == typeof(NowPlaying) select p).SingleOrDefault();

                var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

                if (container != null) container.IsTabStop = false;
                AppShell.Current.NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            };
        }

        /// <summary>
        /// Creates the URI.
        /// </summary>
        /// <param name="temp">The temporary.</param>
        private void createURI(string temp)
        {
            iframe = string.Format("http://player.twitch.tv/?channel={0}", temp);
        }

        /// <summary>
        /// Raises the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var t = e.Parameter.ToString();
            createURI(t);
            strimView.Source = new Uri(iframe);
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