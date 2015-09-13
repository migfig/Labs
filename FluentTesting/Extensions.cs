using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentTesting
{
    /// <summary>
    /// Object extensions
    /// </summary>
    public static class Extensions
    {

#if ENGLISH_LANGUAGE
        /// <summary>
        /// Provides an It object on which apply the methods testing
        /// </summary>
        /// <param name="description">Test description</param>
        /// <param name="target">object instance</param>
        /// <returns>Next object in fluent sequence</returns>
        public static It.OnIt On(this string description, object target)
        {
            return new It(description)
                .On(target);
        }

        public static Instance With(this string description, object target)
        {
            return new Instance(description, target);
        }
#endif

#if SPANISH_LANGUAGE
        /// <summary>
        /// Provides an It object on which apply the methods testing
        /// </summary>
        /// <param name="description">Test description</param>
        /// <param name="target">object instance</param>
        /// <returns>Next object in fluent sequence</returns>
        public static It.OnIt Sobre(this string description, object target)
        {
            return new It(description)
                .On(target);
        }
#endif

        /// <summary>
        /// Converts a name value item into an enumerable collection
        /// </summary>
        /// <param name="item">name value item</param>
        /// <returns>collection of a single item</returns>
        public static IEnumerable<NameValue> Collection(this NameValue item)
        {
            return new List<NameValue> {item};
        }
    }

    /// <summary>
    /// Item storage for properties verification
    /// Property name, value and optionally if it is a property or a method
    /// </summary>
    public class NameValue
    {
        /// <summary>
        /// storage property
        /// </summary>
        private readonly Tuple<MemberTypes, KeyValuePair<string, object>> _property;

        /// <summary>
        /// Property name to be used by reflection
        /// </summary>
        public string PropertyName => _property.Item2.Key;

        /// <summary>
        /// Property value to be used on verfication/assertions
        /// </summary>
        public object PropertyValue => _property.Item2.Value;

        /// <summary>
        /// Member type of this property
        /// </summary>
        public MemberTypes MemberType => _property.Item1;

        /// <summary>
        /// Provide property name, value and optionally the kind of property
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <param name="propertyValue">property value</param>
        /// <param name="memberType">property member type</param>
        public NameValue(string propertyName, object propertyValue, MemberTypes memberType = MemberTypes.Property)
        {
            _property = new Tuple<MemberTypes, KeyValuePair<string, object>>(
                memberType,
                new KeyValuePair<string, object>(propertyName, propertyValue));
        }

        public override string ToString()
        {
            return string.Format("Property Name: {0}, Value: {1}, Member Type: {2}",
                PropertyName,
                PropertyValue.ToString(),
                MemberType.ToString());
        }
    }
}
