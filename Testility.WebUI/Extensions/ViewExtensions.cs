using System.Web.Mvc;

namespace Testility.WebUI.Extensions
{
    public static class ViewExtensions
    {
        public static MvcHtmlString SourceCodesStatusHelper(this HtmlHelper helper)
        {
            TagBuilder builder = new TagBuilder("div");
            if (helper.ViewContext.Controller.TempData["savemessage"] != null)
            {
                TagBuilder saveMessage = new TagBuilder("div");
                saveMessage.MergeAttribute("class", "alert alert-success");
                TagBuilder closebutton = new TagBuilder("a");
                closebutton.MergeAttribute("href", "#");
                closebutton.MergeAttribute("class", "close");
                closebutton.MergeAttribute("data-dismiss", "alert");
                closebutton.MergeAttribute("aria-label", "close");
                closebutton.InnerHtml = "&times;";
                saveMessage.InnerHtml = closebutton.ToString(); 
                saveMessage.InnerHtml += helper.ViewContext.Controller.TempData["savemessage"].ToString();
                builder.InnerHtml += saveMessage.ToString();
            }

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
