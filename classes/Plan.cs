using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskus.classes
{
    public class Plan : Tasks
    {
        public bool? IsDone { get; set; }

        public Plan() 
        {
            Desc = "";
            IsDone = false;
        }
        
    }
}
