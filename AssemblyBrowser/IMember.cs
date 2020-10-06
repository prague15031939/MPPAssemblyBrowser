using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmBrowser
{
    public interface IMember
    {
        string Name { get; set; }
        string Accessor { get; set; }
        string Note { get; set; }
        string StringForm { get; }
    }

}
