using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisWithMe_WebApi.Helpers
{
    public static class EnumHelper<T>
    {
        public static string[] GetEnumDescriptions()
        {
            Type type = typeof(T);
            var names = Enum.GetNames(type);
            return GetEnumDescriptions(names);
        }

        public static string[] GetEnumDescriptions(List<T> enums)
        {
            var names = new string[enums.Count];
            for (int i = 0; i < enums.Count; i++)
            {
                names[i] = enums[i].ToString();
            }

            return GetEnumDescriptions(names);
        }

        private static string[] GetEnumDescriptions(string[] enumNames)
        {
            Type type = typeof(T);
            var descriptions = new String[enumNames.Length];

            var counter = 0;
            foreach (var name in enumNames)
            {
                var field = type.GetField(name);
                var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                descriptions[counter] = customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
                counter++;
            }

            return descriptions;
        }

        public static string[] GetEnumNames()
        {
            Type type = typeof(T);
            return Enum.GetNames(type);
        }

        public static string[] GetEnumNames(List<T> enums)
        {
            var names = new string[enums.Count];
            for (int i = 0; i < enums.Count; i++)
            {
                names[i] = enums[i].ToString();
            }

            return names;
        }

        public static T GetEnumFromDescription(string description)
        {
            Type type = typeof(T);
            var names = Enum.GetNames(type);

            foreach (var name in names)
            {
                var field = type.GetField(name);
                var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var descriptionAttr = customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;

                if (description == descriptionAttr)
                {
                    return (T)Enum.Parse(type, name);
                }
            }

            return default(T);
        }

        public static string GetDescriptionFromEnum(T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            Type type = typeof(T);
            var name = Enum.GetName(type, value);

            if (name == null)
            {
                return string.Empty;
            }

            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }

        public static T GetEnumFromName(string name)
        {
            Type type = typeof(T);
            if (string.IsNullOrWhiteSpace(name) || !Enum.IsDefined(type, name))
            {
                return default(T);
            }
            
            return (T)Enum.Parse(typeof(T), name);
        }

        public static string GetDescriptionFromOptionalEnum(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var enumValue = (T)value;
            return GetDescriptionFromEnum(enumValue);
        }

        public static string GetNameFromDescription(string description)
        {
            return GetEnumFromDescription(description).ToString();
        }

        public static string GetDescriptionFromName(string name)
        {
            return GetDescriptionFromEnum(GetEnumFromName(name));
        }
    }
}
