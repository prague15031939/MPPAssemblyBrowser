using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AsmBrowser
{
    public class AssemblyBrowser
    {
        private BrowserResult result;

        public BrowserResult Browse(string AssemblyPath)
        {
            result = new BrowserResult();
            Assembly asm = Assembly.LoadFrom(AssemblyPath);
            List<Type> AssemblyTypes = asm.GetTypes().ToList();

            foreach (Type type in AssemblyTypes)
            {
                string nsName = type.Namespace;
                if (!result.Namespaces.Exists(obj => obj.FullName == nsName))
                {
                    Namespace ns = new Namespace();
                    ns.FullName = nsName;
                    result.Namespaces.Add(ns);
                }

                Namespace NamespaceItem = result.Namespaces.Single(obj => obj.FullName == nsName);
                string tName = type.FullName;
                if (!NamespaceItem.DataTypes.Exists(obj => obj.FullName == tName))
                {
                    DataType dt = new DataType();
                    dt.FullName = tName;
                    NamespaceItem.DataTypes.Add(dt);
                }

                DataType DataTypeItem = NamespaceItem.DataTypes.Single(obj => obj.FullName == tName);
                AddMember(type, DataTypeItem);
            }

            return result;
        }

        private void AddMember(Type DeclaringType, DataType DataTypeItem)
        {
            foreach (MemberInfo member in DeclaringType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        {
                            AssemblyField field = new AssemblyField();
                            field.Accessor = GetAccessor(member);
                            field.Name = (member as FieldInfo).Name;
                            field.type = (member as FieldInfo).FieldType;
                            DataTypeItem.Members.Add(field);
                        }
                        break;
                    case MemberTypes.Property:
                        {
                            AssemblyProperty property = new AssemblyProperty();
                            property.Accessor = GetAccessor(member);
                            property.Name = (member as PropertyInfo).Name;
                            property.type = (member as PropertyInfo).PropertyType;
                            DataTypeItem.Members.Add(property);
                        }
                        break;
                    case MemberTypes.Method:
                        {
                            if (!typeof(object).GetMethods().ToList().Exists(obj => obj.Name == (member as MethodInfo).Name))
                            {
                                AssemblyMethod method = new AssemblyMethod();
                                method.Accessor = GetAccessor(member);
                                method.Name = (member as MethodInfo).Name;
                                method.ReturnType = (member as MethodInfo).ReturnType;
                                method.Parameters = (member as MethodInfo).GetParameters().ToList();
                                DataTypeItem.Members.Add(method);
                            }
                        }
                        break;
                }
            }
        }

        public static string GetAccessor(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Field && (member as FieldInfo).IsPublic ||
                member.MemberType == MemberTypes.Property && (member as PropertyInfo).GetSetMethod() != null ||
                member.MemberType == MemberTypes.Method && (member as MethodInfo).IsPublic)
                return "public";
            return "private";
        }
    }
}
