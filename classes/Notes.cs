using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Taskus.classes
{
    public class Notes : Tasks
    {
        public bool? ToPin { get; set; }

        public Notes() { ToPin = false; }

    }
}
