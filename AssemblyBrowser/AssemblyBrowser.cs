using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AsmBrowser
{
    public class AssemblyBrowser
    {
        private BrowserResult result;
        private delegate void AddingMethod(MemberInfo member, DataType DataTypeItem);
        private Dictionary<MemberTypes, AddingMethod> AddingMethods;
        private List<MethodInfo> ExtensionMethods = new List<MethodInfo>();

        public AssemblyBrowser()
        {
            AddingMethods = new Dictionary<MemberTypes, AddingMethod>();
            AddingMethods.Add(MemberTypes.Field, AddField);
            AddingMethods.Add(MemberTypes.Property, AddProperty);
            AddingMethods.Add(MemberTypes.Method, AddMethod);
        }

        public BrowserResult Browse(string AssemblyPath)
        {
            result = new BrowserResult();
            Assembly asm = Assembly.LoadFrom(AssemblyPath);
            result.FullName = asm.FullName;
            List<Type> AssemblyTypes = asm.GetTypes().ToList();

            foreach (Type type in AssemblyTypes)
            {
                string nsName = type.Namespace;
                if (!result.Namespaces.ToList().Exists(obj => obj.Name == nsName))
                {
                    Namespace ns = new Namespace();
                    ns.Name = nsName;
                    result.Namespaces.Add(ns);
                }

                Namespace NamespaceItem = result.Namespaces.Single(obj => obj.Name == nsName);
                DataType dt = new DataType();
                dt.Name = type.Name;
                dt.FullName = type.FullName;
                NamespaceItem.DataTypes.Add(dt);

                DataType DataTypeItem = NamespaceItem.DataTypes.Single(obj => obj.FullName == type.FullName);
                AddMember(type, DataTypeItem);
            }

            foreach (MethodInfo method in ExtensionMethods)
                AddExtensionMethod(method);

            return result;
        }

        private void AddMember(Type type, DataType DataTypeItem)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static;
            foreach (MemberInfo member in type.GetMembers(flags))
            {
                if (AddingMethods.ContainsKey(member.MemberType))
                    AddingMethods[member.MemberType](member, DataTypeItem);
            }
        }

        private void AddField(MemberInfo member, DataType DataTypeItem)
        {
            AssemblyDataMember field = new AssemblyDataMember();
            field.Note = "f";
            field.Accessor = GetAccessor(member);
            field.Name = (member as FieldInfo).Name;
            field.type = (member as FieldInfo).FieldType;
            DataTypeItem.Members.Add(field);
        }

        private void AddProperty(MemberInfo member, DataType DataTypeItem)
        {
            AssemblyDataMember property = new AssemblyDataMember();
            property.Note = "p";
            property.Accessor = GetAccessor(member);
            property.Name = (member as PropertyInfo).Name;
            property.type = (member as PropertyInfo).PropertyType;
            DataTypeItem.Members.Add(property);
        }

        private void AddMethod(MemberInfo member, DataType DataTypeItem)
        {
            if (isExtensionMethod(member as MethodInfo))
            {
                ExtensionMethods.Add(member as MethodInfo);
                return;
            }
            AddDefaultMethod(member, DataTypeItem, "m");
        }

        private void AddDefaultMethod(MemberInfo member, DataType DataTypeItem, string Note)
        {
            AssemblyMethod method = new AssemblyMethod();
            method.Note = Note;
            method.Accessor = GetAccessor(member);
            method.Name = (member as MethodInfo).Name;
            method.ReturnType = (member as MethodInfo).ReturnType;
            method.Parameters = (member as MethodInfo).GetParameters().ToList();
            DataTypeItem.Members.Add(method);
        }

        private void AddExtensionMethod(MethodInfo method)
        {
            Type ExpandableType = method.GetParameters()[0].ParameterType;
            foreach (Namespace ns in result.Namespaces)
            { 
                if (ns.DataTypes.ToList().Exists(obj => obj.FullName == ExpandableType.FullName))
                {
                    DataType dt = ns.DataTypes.Single(obj => obj.FullName == ExpandableType.FullName);
                    AddDefaultMethod(method, dt, "ext");
                    return;
                }
            }
        }

        public static bool isExtensionMethod(MethodInfo method)
        {
            if (method.IsDefined(typeof(ExtensionAttribute), false) && method.DeclaringType.IsDefined(typeof(ExtensionAttribute), false))
                return true;
            return false;
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
