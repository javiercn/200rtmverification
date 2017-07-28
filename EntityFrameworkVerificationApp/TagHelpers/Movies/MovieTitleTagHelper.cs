using EntityFrameworkVerificationApp.Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.TagHelpers.Movies
{
    public class MovieTitleTagHelper : TagHelper
    {
        private const string MovieId = "movie-id";

        public MovieTitleTagHelper(CatalogContext catalog)
        {
            Catalog = catalog;
        }

        [HtmlAttributeNotBound]
        public CatalogContext Catalog { get; }

        [HtmlAttributeName(MovieId)]
        public int Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var movie = await Catalog.Movies.FindAsync(Id);

            output.Content.SetContent(movie.Title);
            output.TagName = "span";
        }
    }
}
