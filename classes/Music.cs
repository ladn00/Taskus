using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskus.classes
{
    public class Music : Tasks
    {
        public Music() { }

        public string Link { get; set; }

        public string ImagePath { 
            
            get
            {
                string result = "..//imgs/itunes-note.png";
                if (Link.Contains("youtube"))
                {
                    string обрезыш = Link.Replace("https://www.youtube.com/watch?v=", "");
                    result = $"https://i.ytimg.com/vi/{обрезыш}/hqdefault.jpg";
                }
                else if (Link.Contains("open.spotify.com"))
                {
                    result = $"..//imgs/spotify.png";
                }
                return result;
            } 
        }
    }
}
