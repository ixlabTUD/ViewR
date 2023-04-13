using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ViewR.HelpersLib.Extensions.General
{
    /// <summary>
    /// Extension methods for string.
    /// 
    /// Modified, from Andrew Perry @ https://gist.github.com/omgwtfgames/f917ca28581761b8100f/564dddacb77d63d0e8084f7cdc0cd16f86e82f9c - Thanks!
    /// And
    /// Modified, from Atori708 @ https://gist.github.com/atori708/ecf61e674141695a910958e183f41d3d - Thanks!
    /// </summary>

    public static class StringExtensionMethods
    {
        #region Modified and extended from Atori708

        /// <summary>
        /// Start a string with reference to its class!
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"> should be this.GetType() usually </param>
        /// <returns></returns>
        public static string StartWithFrom(this string str, Type type) => "<b>" + type + "</b>: " + str;
        public static string StartWithFrom(this string str, string type) => "<b>" + type + "</b>: " + str;
        
        public static string Bold(this string str) => "<b>" + str + "</b>";

        public static string Italic(this string str) => "<i>" + str + "</i>";

        public static string Size(this string str, int size) => $"<size=\"{size}\">" + str + "</size>";

        public static string Aqua(this string str) => AddColorTag(str, "aqua");

        public static string Blue(this string str) => AddColorTag (str, "blue");

        public static string Black(this string str) => AddColorTag (str, "black");

        public static string Brown(this string str) => AddColorTag (str, "brown");

        public static string Cyan(this string str) => AddColorTag (str, "cyan");

        public static string DarkBlue(this string str) => AddColorTag (str, "darkblue");

        public static string Fuchsia(this string str) => AddColorTag (str, "fuchsia");

        public static string Green(this string str) => AddColorTag (str, "green");

        public static string Grey(this string str) => AddColorTag (str, "grey");

        public static string LightBlue(this string str) => AddColorTag (str, "lightblue");

        public static string Lime(this string str) => AddColorTag (str, "lime");

        public static string Magenta(this string str) => AddColorTag (str, "magenta");

        public static string Maroon(this string str) => AddColorTag (str, "maroon");

        public static string Navy(this string str) => AddColorTag (str, "navy");

        public static string Olive(this string str) => AddColorTag (str, "olive");

        public static string Orange(this string str) => AddColorTag (str, "orange");

        public static string Purple(this string str) => AddColorTag (str, "purple");

        public static string Red(this string str) => AddColorTag (str, "red");

        public static string Silver(this string str) => AddColorTag (str, "silver");

        public static string Teal(this string str) => AddColorTag (str, "teal");

        public static string White(this string str) => AddColorTag (str, "white");

        public static string Yellow(this string str) => AddColorTag (str, "yellow");

        public static string Color(this string str, Color color) => AddColorTag (str, color);
        
        public static string Color(this string str, string color) => AddColorTag (str, color);

        private static string AddColorTag(string str, string color) => $"<color=\"{color}\">" + str + "</color>";
        
        private static string AddColorTag(string str, Color color) =>
            ($"<color=#{((byte) (color.r * 255f)):X2}{((byte) (color.r * 255f)):X2}{((byte) (color.r * 255f)):X2}>" + str + "</color>");
 
        #endregion

        #region Extend string by warnings

        public enum WarningType
        {
            EditorOnly
        }

        public static string ExtendByWarning(this string str, WarningType warningType)
        {
            return str + " -- " + warningType switch
            {
                WarningType.EditorOnly => "This method is for EDITOR ONLY use.".Lime(),
                _ => warningType.ToString().Lime(),
            };
        }


        #endregion

        
        #region Modified and extended from Andrew Perry

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    
        // Named format strings from object attributes. Eg:
        // string blaStr = aPerson.ToString("My name is {FirstName} {LastName}.")
        // From: http://www.hanselman.com/blog/CommentView.aspx?guid=fde45b51-9d12-46fd-b877-da6172fe1791
        public static string ToString(this object anObject, string aFormat) => ToString(anObject, aFormat, null);

        public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            var type = anObject.GetType();
            var reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
            var mc = reg.Matches(aFormat);
            var startIndex = 0;
            foreach (Match m in mc)
            {
                var g = m.Groups[2]; //it's second in the match between { and }
                var length = g.Index - startIndex - 1;
                sb.Append(aFormat.Substring(startIndex, length));

                string toGet;
                var toFormat = string.Empty;
                var formatIndex = g.Value.IndexOf(":", StringComparison.Ordinal); //formatting would be to the right of a :
                if (formatIndex == -1) //no formatting, no worries
                {
                    toGet = g.Value;
                }
                else //pickup the formatting
                {
                    toGet = g.Value.Substring(0, formatIndex);
                    toFormat = g.Value.Substring(formatIndex + 1);
                }

                //first try properties
                var retrievedProperty = type.GetProperty(toGet);
                Type retrievedType = null;
                object retrievedObject = null;
                if (retrievedProperty != null)
                {
                    retrievedType = retrievedProperty.PropertyType;
                    retrievedObject = retrievedProperty.GetValue(anObject, null);
                }
                else //try fields
                {
                    var retrievedField = type.GetField(toGet);
                    if (retrievedField != null)
                    {
                        retrievedType = retrievedField.FieldType;
                        retrievedObject = retrievedField.GetValue(anObject);
                    }
                }

                if (retrievedType != null) //Cool, we found something
                {
                    string result;
                    if (toFormat == string.Empty) //no format info
                    {
                        result = retrievedType.InvokeMember("ToString",
                            BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                            , null, retrievedObject, null) as string;
                    }
                    else //format info
                    {
                        result = retrievedType.InvokeMember("ToString",
                            BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                            , null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
                    }
                    sb.Append(result);
                }
                else //didn't find a property with that name, so be gracious and put it back
                {
                    sb.Append("{");
                    sb.Append(g.Value);
                    sb.Append("}");
                }
                startIndex = g.Index + g.Length + 1;
            }
            if (startIndex < aFormat.Length) //include the rest (end) of the string
            {
                sb.Append(aFormat.Substring(startIndex));
            }
            return sb.ToString();
        }

        #endregion

        #region Extend by DateTime

        /// <summary>
        /// Appends the given string by the current DateTime.
        /// </summary>
        /// <param name="format">The <see cref="DateTime"/> format to use. Uses "sortable" by default.</param>
        /// <param name="divider">A potential divider between string and Date</param>
        /// <returns></returns>
        public static string ExtendByDateTimeNow(this string str, string format = "s", string divider = "-")
        {
            var stringBuilder = new StringBuilder(str);
            stringBuilder.Append(divider);
            stringBuilder.Append(DateTime.Now.ToString(format));
            return stringBuilder.ToString();
        }

        #endregion
    }
}