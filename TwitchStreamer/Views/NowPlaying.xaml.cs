using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.Media.Streaming;
using Windows.Media.Transcoding;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TwitchStreamer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NowPlaying : Page
    {
        StringBuilder html = new StringBuilder();
        Uri source;
        Uri chat;
        
        private string twitch = "src='http://player.twitch.tv/?channel='";
        private string[] tc = { "http://www.twitch.tv/", "", "/chat" };
        List<Task> chatStream = new List<Task>();
        CancellationTokenSource cancelSource;
        public NowPlaying()
        {
            this.InitializeComponent();
            this.Loading += (sender, args) =>
            {
                initChatStream();
            };

            this.Loaded += (sender, args) =>
            {
                
                var item = (from p in AppShell.Current.navlist where p.DestinationPage == typeof(NowPlaying) select p).SingleOrDefault();

                var container = (ListViewItem)AppShell.Current.NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                AppShell.Current.NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            };
            
        }

       private void getChatURi(string temp)
        {
            tc[1] = temp;
            var ch = string.Empty;
            foreach (var f in tc){ ch += f; }
            ch = "http://www.twitch.tv/" + temp + "/chat";
            chat = new Uri(ch);
        }
        private void createURI(string temp)
        {
            string streamURL = string.Empty;
            streamURL = "http:\\player.twitch.tv/?channel=pianoimproman";
            twitch += temp;
            html.Append("<!DOCTYPE html>").AppendLine();
            html.Append("<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>").AppendLine();
            html.Append("<head>").AppendLine();
            html.Append("<meta charset='utf - 8' />").AppendLine();
            html.Append("<title></title>").AppendLine();
            html.Append("</head>").AppendLine();
            html.Append("<body>").AppendLine();
            html.Append("<iframe").AppendLine();
            html.Append("src='http:\\player.twitch.tv/?channel={grandgrant}'").AppendLine();
            //html.Append(streamURL);
            //html.Append("'");
            html.Append("height = '720'").AppendLine();
            html.Append("width = '1280'").AppendLine();
            html.Append("frameborder = '0'").AppendLine();
            html.Append("scrolling='no'").AppendLine();
            html.Append("allowfullscreen>").AppendLine();
            html.Append("</iframe>").AppendLine();
            html.Append("</body>").AppendLine();
            html.Append("</html>").AppendLine();


            //source = new Uri(twitch);
        }
        
        private void startStream()
        {
            sView.NavigateToString(html.ToString());   
            //sView.Source = source;         
        }
        private void startChat()
        {
            //chatView.Navigate(chat);
            //chatView.Source = chat;
        }
        private void initChatStream()
        {
            //IMediaSource temp;
            
            //mediaElement.SetMediaStreamSource(new IMediaSource)
            cancelSource = new CancellationTokenSource();
            //var t = Task.Factory.StartNew(new Action(startStream), TaskCreationOptions.LongRunning).ContinueWith(tsk =>
            //{
            //    var broken = tsk.Exception.Flatten();

            //    broken.Handle(ex => { var e = show(); return true; });
            //}, TaskContinuationOptions.OnlyOnFaulted);
            //var c = Task.Factory.StartNew(new Action(startChat), TaskCreationOptions.LongRunning);
            //chatStream.Add(t);
            //chatStream.Add(c);
            //t.RunSynchronously();
            //c.RunSynchronously();
            //Task.Factory.StartNew(new Action(startStream), TaskCreationOptions.LongRunning);
            //Task.Factory.StartNew(new Action(startChat), TaskCreationOptions.LongRunning);
            //Task.WaitAll(chatStream.ToArray());
            //var f = new Task(new Action(startStream), cancellationToken: cancelSource.Token,
                             //   creationOptions: TaskCreationOptions.LongRunning);
            //f.RunSynchronously();
            //startStream();
        }
        private async Task<bool> show()
        {
            MessageDialog error = new MessageDialog("Task was not completed at like 75!");
            await error.ShowAsync();
            return true;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            //base.OnNavigatedTo(e);
           // var t = e.Parameter.ToString();
            //createURI(t);
            //getChatURi(t);
            
        }

        //public void runStream()
        //{
        //    initChatStream();
        //    foreach(var item in chatStream)
        //    {
        //        item.RunSynchronously();
        //    }
        //}

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

        private void sView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //sView.Visibility = Visibility.Visible;
            //chatView.Visibility = Visibility.Visible;
            
        }

        private void sView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            //sView.Visibility = Visibility.Collapsed;
            
        }

        private void PlayRoot_LostFocus(object sender, RoutedEventArgs e)
        {
            //sView.Stop();
            //chatView.Stop();
        }

        //private void toggleButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //switch (chatView.Visibility)
        //    //{
        //    //    case Visibility.Collapsed:
        //    //        chatView.Navigate(chat);
        //    //        chatView.Visibility = Visibility.Visible;                    
        //    //        return;
        //    //    case Visibility.Visible:
        //    //        chatView.Visibility = Visibility.Collapsed;
        //    //        chatView.Stop();
        //    //        return;
        //    //    default:
        //    //        return;
                
        //    //}
        //    //chatButton.IsChecked = true ? true : false;
        //}

            //private void chatToggleButton_Click(object sender, RoutedEventArgs e)
            //{

            //    switch (chatView.Visibility)
            //    {
            //        case Visibility.Collapsed:
            //            chatView.Navigate(chat);
            //            chatView.Visibility = Visibility.Visible;
            //            return;
            //        case Visibility.Visible:
            //            chatView.Visibility = Visibility.Collapsed;
            //            chatView.Stop();
            //            return;
            //        default:
            //            return;
            //    }

            //}
        }

}
