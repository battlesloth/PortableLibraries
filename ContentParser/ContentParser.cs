using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MarkdownDeep;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContentParser
{
    internal class SectionTypes
    {
        internal static string Divider = "divider";
        internal static string Items = "items";
        internal static string Table = "table";
        internal static string List = "list";
        internal static string Header = "header";
    }


    public class ContentParser
    {
        private static readonly Regex propertyRegex = new Regex(@"\[(\w+)\]");

        public static string ConvertToHtml(string data, string style)
        {

            dynamic styleObject;
            dynamic dataObject;

            var md = new Markdown();
            var output = new StringBuilder();

            try
            {
                styleObject = JsonConvert.DeserializeObject(style);
                dataObject = JsonConvert.DeserializeObject<ExpandoObject>(data);
            }
            catch (JsonSerializationException ex)
            {
                return $"<p>Error deserializing a parameter.  ex:{ex.Message}</p>";
            }

            foreach (string section in styleObject.sectionorder)
            {
                if (styleObject.sections[section].type == SectionTypes.Divider)
                {
                    output.Append("<hr/>");
                }
                else if (styleObject.sections[section].type == SectionTypes.Header)
                {
                    foreach (string item in styleObject.sections[section].itemorder)
                    {
                        output.Append(md.Transform((string) styleObject.sections[section].items[item]));
                    }
                }
                else if (styleObject.sections[section].type == SectionTypes.List)
                {
                    foreach (
                        KeyValuePair<string, object> listItem in
                            (IDictionary<string, object>) ((IDictionary<string, object>) dataObject)[section])
                    {
                        var format = (string) styleObject.sections[section].format;

                        output.Append(md.Transform(format.Replace("[key]", listItem.Key)
                            .Replace("[value]", listItem.Value.ToString())));
                    }
                }
                else if (styleObject.sections[section].type == SectionTypes.Items)
                {
                    foreach (string item in styleObject.sections[section].itemorder)
                    {
                        var itemString = (string) styleObject.sections[section].items[item];

                        foreach (Match match in propertyRegex.Matches(itemString))
                        {
                            itemString = itemString.Replace(match.Groups[0].Value,
                                (string) ((IDictionary<string, object>) dataObject)[match.Groups[1].Value]);
                        }

                        output.Append(md.Transform(itemString));
                    }
                }
            }

            return output.ToString();
        }

    }
}
