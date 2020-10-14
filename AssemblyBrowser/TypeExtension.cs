using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmBrowser
{
    public static class TypeExtension
    {
        public static string CustomGetType(this Type type)
        {
            if (!type.IsGenericType)
                return $"{type.Name}";

            StringBuilder builder = new StringBuilder();
            Type definition = type.GetGenericTypeDefinition();
            int endIndex = definition.Name.IndexOf('`');
            builder.Append(definition.Name.Substring(0, endIndex));
            builder.Append("<");

            List<Type> argTypes = type.GetGenericArguments().ToList();
            foreach (Type t in argTypes)
            {
                if (argTypes.IndexOf(t) != 0)
                    builder.Append(", ");
                builder.Append(t.CustomGetType());
            }
            builder.Append(">");
            return builder.ToString();
        }
    }
}
