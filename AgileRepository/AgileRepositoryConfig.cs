using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Repository
{
    public class AgileRepositoryConfig
    {
        public string[] AssbemlyNames { get; set; }

        public Action<string,object> SqlMonitor { get; set; }
    }
}
