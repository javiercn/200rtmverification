using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;

namespace EntityFrameworkVerificationApp.TagHelpers.Layout
{
    [HtmlTargetElement("wrap-panel")]
    public class WrapPanelTagHelper : TagHelper
    {
        private static readonly char[] Separator = new char[] { ' ' };

        [HtmlAttributeName("tag-name")]
        public string Tag { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var index = output.Attributes.IndexOfName("class");
            var classAttribute = index != -1 ?
                UpdateClassAttribute(output.Attributes[index]) :
                new TagHelperAttribute("class", "wrap-panel");

            output.TagName = Tag ?? "div";
            if (index != -1)
            {
                output.Attributes.Remove(output.Attributes[index]);
            }

            output.Attributes.Add(classAttribute);
        }

        private static TagHelperAttribute UpdateClassAttribute(TagHelperAttribute attribute) =>
            new TagHelperAttribute("class", AppendClass((HtmlString)attribute.Value));

        private static string AppendClass(HtmlString value) =>
            string.Join(' ', value.Value.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Append("wrap-panel"));
    }
}
