namespace HazGo.BuildingBlocks.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public static class EnumExtension
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }

        public static Dictionary<string, short> GetEnumValuesAndDescriptions<T>()
            where T : Enum
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T is not System.Enum");
            }

            Dictionary<string, short> types = Enum.GetValues(enumType)
                                       .Cast<T>()
                                       .ToDictionary(k => k.GetEnumDescription(), v => Convert.ToInt16(v));
            return types;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(
                    field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.ToLower() == description.ToLower())
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }

        public static List<EnumValue> GetValues<T>()
            where T : Enum
        {
            List<EnumValue> values = new List<EnumValue>();
            Type enumType = typeof(T);
            foreach (var itemType in Enum.GetValues(enumType).Cast<T>())
            {
                values.Add(new EnumValue()
                {
                    Text = itemType.GetEnumDescription(),
                    Value = Convert.ToInt16(itemType),
                });
            }

            return values.OrderBy(_ => _.Text).ToList();
        }

        public static LookupList ToLookupList<T>()
            where T : System.Enum
        {
            List<LookupItem> values = new List<LookupItem>();
            Type enumType = typeof(T);
            foreach (var itemType in Enum.GetValues(enumType).Cast<T>())
            {
                values.Add(new LookupItem()
                {
                    DisplayName = itemType.GetEnumDescription(),
                    Value = itemType.ToString(),
                });
            }

            return new LookupList()
            {
                Code = enumType.Name,
                Items = values,
            };
        }
    }

    public class EnumValue
    {
        public string Text { get; set; }

        public short Value { get; set; }
    }

    public class LookupItem
    {
        public string DisplayName { get; set; }

        public string Value { get; set; }
    }

    public class LookupList
    {
        public string Code { get; set; }

        public List<LookupItem> Items { get; set; }
    }
}
