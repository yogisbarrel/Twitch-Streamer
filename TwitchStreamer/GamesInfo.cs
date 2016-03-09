using System;
using System.IO;

using System.Collections.Specialized;

using Windows.UI.Xaml.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Windows.Web.Http;
using Windows.Web.Http.Headers;




namespace TwitchStreamers.Views
{
    public class GamesInfo
    {
        Uri api = new Uri("https://api.twitch.tv/kraken/games/top");
        public string games;
        //private HttpClient httpClient = new HttpClient();
        //private HttpResponseMessage response;
        //private WebRequest request;
        //private WebResponse response;
        //public RootObject deser;
        //public PortObject gms;

        public Collection<OrderedDictionary> results = new Collection<OrderedDictionary>();
        //public int gameNumber;
        //private PowerShell ps = PowerShell.Create();
        private string gamesFunction = string.Empty;
        public GamesInfo()
        {
            //GetAsync();
            
            

        }
        //async public Task<RootObject> GetAsync()
        //{
        //    string test;
        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.GetAsync(api);
        //        response.EnsureSuccessStatusCode();
        //        test = await response.Content.ReadAsStringAsync();
        //    }

        //    //deser = JsonConvert.DeserializeObject<RootObject>(test);


        //    return deser;
        //}

        public void RunAsync(string temp)
        {
            //games = await retJson();
            //deser = JsonConvert.DeserializeObject<RootObject>(temp);

        }



        //private async Task<string> getJson()
        //{
        //    var tempResponse = await request.GetResponseAsync();
        //    using (Stream responseStream = tempResponse.GetResponseStream())
        //    {
        //        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
        //        return reader.ReadToEnd();
        //    }

        //}
        private ImageBrush conv(string link)
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


        public class Links
        {
            public string self { get; set; }
            public string next { get; set; }
        }

        public class Box
        {
            public string large { get; set; }
            public string medium { get; set; }
            public string small { get; set; }
            public string template { get; set; }
        }

        public class Logo
        {
            public string large { get; set; }
            public string medium { get; set; }
            public string small { get; set; }
            public string template { get; set; }
        }

        public class Links2
        {
        }

        public class Game
        {
            public string name { get; set; }
            public Box box { get; set; }
            public Logo logo { get; set; }
            public Dictionary<string, string> _links { get; set; }
            public int _id { get; set; }
            public int giantbomb_id { get; set; }
        }

        public class Top
        {
            public Game game { get; set; }
            public int viewers { get; set; }
            public int channels { get; set; }
        }

        public class RootObject
        {
            public Dictionary<string, string> _links { get; set; }
            public int _total { get; set; }
            public List<Top> top { get; set; }
        }
    }
}



