using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AsmBrowser
{
    public class BrowserResult
    {
        public string FullName { get; set; }
        public ObservableCollection<Namespace> Namespaces { get; } = new ObservableCollection<Namespace>();
    }

    public class Namespace
    { 
        public string FullName { get; set; }
        public ObservableCollection<DataType> DataTypes { get; } = new ObservableCollection<DataType>();
    }

    public class DataType
    {
        public string FullName { get; set; }
        public ObservableCollection<Member> Members { get; } = new ObservableCollection<Member>();
    }

    public class Member
    {
        public string Name { get; set; }
        public string Accessor { get; set; }
    }

    public class AssemblyField : Member
    {
        public Type type { get; set; }
    }

    public class AssemblyProperty : Member
    {
        public Type type { get; set; }
    }

    public class AssemblyMethod : Member
    {
        public Type ReturnType { get; set; }
        public List<ParameterInfo> Parameters;
    }

}
