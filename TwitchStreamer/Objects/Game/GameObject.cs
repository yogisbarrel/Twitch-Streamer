using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchStreamer.Objects.Game
{
    /// <summary>
    /// 
    /// </summary>
    public class RootObject
    {
        public Dictionary<string, string> _links { get; set; }
        public int _total { get; set; }
        public List<Top> top { get; set; }
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

    
}
