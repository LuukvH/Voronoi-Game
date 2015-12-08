using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voronoi
{
    class LogEntry
    {
        public LogEntry(String text)
        {
            this.Message = text;
        }

        public String Message { get; private set; }
        public List<Object> objects = new List<Object>();
    }
}
