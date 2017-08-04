using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.TagHelpers
{
    public class PartialTagHelper : TagHelper
    {
        public PartialTagHelper(IHtmlHelper html)
        {
            Html = html;
        }

        public ModelExpression For { get; set; }

        public string Template { get; set; }

        public object Value { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public IHtmlHelper Html { get; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            ((IViewContextAware)Html).Contextualize(ViewContext);

            if (For != null)
            {
                var viewDataType = typeof(ViewDataDictionary<>).MakeGenericType(For.ModelExplorer.ModelType);
                var viewData = (ViewDataDictionary)Activator.CreateInstance(
                    viewDataType,
                    ViewContext.ViewData,
                    Value);
                viewData.TemplateInfo.HtmlFieldPrefix = For.Name;
                output.Content.SetHtmlContent(await Html.PartialAsync(Template, Value, viewData));
            }
            else
            {
                output.Content.SetHtmlContent(await Html.PartialAsync(Template, Value));
            }
        }
    }
}
