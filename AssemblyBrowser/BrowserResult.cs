using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
        public ObservableCollection<IMember> Members { get; } = new ObservableCollection<IMember>();
    }

    public class AssemblyField : IMember
    {
        public Type type { get; set; }
        public string Name { get; set; }
        public string Accessor { get; set; }
        public string StringForm
        {
            get
            {
                return $"{Accessor} {type.Name} {Name}";
            }
        }
    }

    public class AssemblyProperty : IMember
    {
        public Type type { get; set; }
        public string Name { get; set; }
        public string Accessor { get; set; }
        public string StringForm
        {
            get
            {
                return $"{Accessor} {type.Name} {Name}";
            }
        }
    }

    public class AssemblyMethod : IMember
    {
        public Type ReturnType { get; set; }
        public string Name { get; set; }
        public string Accessor { get; set; }

        public List<ParameterInfo> Parameters;
        public string StringForm
        {
            get
            {
                StringBuilder builder = new StringBuilder("(");
                foreach (ParameterInfo param in Parameters)
                {
                    if (Parameters.IndexOf(param) != 0)
                        builder.Append(", ");
                    builder.Append($"{param.ParameterType.Name} {param.Name}");
                }
                builder.Append(")");
                return $"{Accessor} {ReturnType.Name} {Name}" + builder.ToString();
            }
        }
    }

}
