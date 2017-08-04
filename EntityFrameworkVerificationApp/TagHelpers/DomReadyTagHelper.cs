using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.TagHelpers
{
    [HtmlTargetElement("script", Attributes = "dom-ready")]
    public class DomReadyTagHelper : TagHelper
    {
        private const string Prefix = @"document.addEventListener(""DOMContentLoaded"", function () {";
        private const string Suffix = @"});";

        public DomReadyTagHelper(IHtmlHelper html)
        {
            Html = html;
        }

        public IHtmlHelper Html { get; }

        public bool DomReady { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            output.PreContent.SetHtmlContent(Prefix);
            output.Content.SetHtmlContent(childContent);
            output.PostContent.SetHtmlContent(Suffix);
        }
    }
}
