using BookStore.WebUI.Models; //35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc; //34

namespace BookStore.WebUI.HTMLHelper
{
    public static class PagingHelper //33
    {
        public static MvcHtmlString PageLinks( //34,35
                       this HtmlHelper Html,
                       PagingInfo pageInfo,
                       Func<int, string> PageUrl)
        {
            StringBuilder result = new StringBuilder(); //36
            for(int i = 1 ; i <= pageInfo.TotalPages ; i++) //37
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", PageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pageInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default"); //38
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create (result.ToString()); //39
        }
    }
}