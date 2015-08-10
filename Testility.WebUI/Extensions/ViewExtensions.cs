using System.Web.Mvc;

namespace Testility.WebUI.Extensions
{
    public static class ViewExtensions
    {
        public static MvcHtmlString SourceCodesStatusHelper(this HtmlHelper helper)
        {
        TagBuilder builder =  new TagBuilder("div");
                if ( helper.ViewContext.Controller.TempData["savemessage"] != null)
                    {
                        TagBuilder saveMessage = new TagBuilder("div");
                        saveMessage.MergeAttribute("class", "alert alert-success");
                        saveMessage.InnerHtml = helper.ViewContext.Controller.TempData["savemessage"].ToString();
                        builder.InnerHtml += saveMessage.ToString();   
                }
                if (helper.ViewContext.Controller.TempData["errormessage"] != null)
                    {
                        TagBuilder errorMessage = new TagBuilder("div");
                        errorMessage.MergeAttribute("class", "alert alert-danger");
                        errorMessage.InnerHtml = helper.ViewContext.Controller.TempData["errormessage"].ToString();
                        builder.InnerHtml += errorMessage.ToString();   
                    }               

                return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString SourceCodesCreateAndEditHeaderHelper(this HtmlHelper helper)
        {
            TagBuilder builder = new TagBuilder("div");
            if (helper.ViewContext.Controller.TempData["header"] != null)
            {
                TagBuilder header = new TagBuilder("h2");
                header.InnerHtml = helper.ViewContext.Controller.TempData["header"].ToString();
                builder.InnerHtml += header.ToString();
            }
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
