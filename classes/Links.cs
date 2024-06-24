using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Taskus.classes
{
    public class Links : Tasks
    {
        public string ImagePath
        {
            get
            {
                string result = "..//imgs/link-solid.png";
                if (Name.Contains("youtube"))
                {
                    string обрезыш = Name.Replace("https://www.youtube.com/watch?v=", "");
                    result = $"https://i.ytimg.com/vi/{обрезыш}/hqdefault.jpg";
                }
                else if (Name.Contains("github"))
                {
                    result = $"..//imgs/github.png";
                }
                else if (Name.Contains("metanit"))
                {
                    result = $"..//imgs/metanit.png";
                }
                else if (Name.Contains("habr"))
                {
                    result = $"..//imgs/habr.png";
                }
                else if (Name.Contains("pdf"))
                {
                    result = $"..//imgs/file-pdf-regular.png";
                }
                return result;
                
            }
        }
    }
}
