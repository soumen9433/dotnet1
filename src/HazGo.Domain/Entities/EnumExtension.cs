namespace HazGo.BuildingBlocks.Common.Extensions
{
    using HazGo.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    public static class EnumExtension
    {
        /// <summary>
        /// Function to get descriotion of enums
        /// </summary>
        /// <param name="enumValue">Its a value of enum member</param>
        /// <returns>Sting value</returns>
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }

        /// <summary>
        /// Gets the enum values and descriptions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">T is not System.Enum</exception>
        public static Dictionary<string, short> GetEnumValuesAndDescriptions<T>() where T : Enum
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not System.Enum");

            Dictionary<string, short> types = Enum.GetValues(enumType)
                                       .Cast<T>()
                                       .ToDictionary(k => k.GetEnumDescription(), v => Convert.ToInt16(v));
            return types;
        }

        /// <summary>
        /// Gets the value from description.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException">Not found. - description</exception>
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.ToLower() == description.ToLower())
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
        }
        public static List<EnumValue> GetValues<T>() where T : Enum
        {
            List<EnumValue> values = new List<EnumValue>();
            Type enumType = typeof(T);
            foreach (var itemType in Enum.GetValues(enumType).Cast<T>())
            {
                //For each value of this enumeration, add a new EnumValue instance
                values.Add(new EnumValue()
                {
                    Text = itemType.GetEnumDescription(),
                    Value = Convert.ToInt16(itemType)
                });
            }
            return values.OrderBy(_ => _.Text).ToList();
        }
    }
    public class EnumValue
    {
        public string Text { get; set; }
        public short Value { get; set; }
    }

}
