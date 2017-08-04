using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;

namespace EntityFrameworkVerificationApp.TagHelpers.Layout
{
    [HtmlTargetElement("wrap-panel-item", ParentTag = "wrap-panel")]
    public class WrapPanelItemTagHelper : TagHelper
    {
        private const string WrapPanelItem = "wrap-panel-item";
        private static readonly char Separator = ' ';

        [HtmlAttributeName("tag-name")]
        public string Tag { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var index = output.Attributes.IndexOfName("class");
            var classAttribute = index != -1 ?
                UpdateClassAttribute(output.Attributes[index]) :
                new TagHelperAttribute("class", WrapPanelItem);

            output.TagName = Tag ?? "div";
            if (index != -1)
            {
                output.Attributes.Remove(output.Attributes[index]);
            }

            output.Attributes.Add(classAttribute);
        }

        private TagHelperAttribute UpdateClassAttribute(TagHelperAttribute attribute) =>
            new TagHelperAttribute("class", AppendClass((HtmlString)attribute.Value));

        private string AppendClass(HtmlString value) =>
            string.Join(' ', value.Value.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Append(WrapPanelItem));
    }
}
