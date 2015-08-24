using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Testility.WebUI.Extensions
{
    public static class HtmlExtensions
    {
        internal static MvcHtmlString DisplayNameHelper(ModelMetadata metadata, string htmlFieldName)
        {
            string resolvedDisplayName = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            return new MvcHtmlString(HttpUtility.HtmlEncode(resolvedDisplayName));
        }

        public static IHtmlString DisplayNameFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IEnumerable<TClass>>> expression1, Expression<Func<TClass, TProperty>> expression2)
        {
            return DisplayNameHelper(ModelMetadata.FromLambdaExpression(expression2, new ViewDataDictionary<TClass>()),
                                     ExpressionHelper.GetExpressionText(expression2));
        }

        internal static MvcHtmlString LabelFor(string forName, ModelMetadata metadata, string htmlFieldName, object htmlAttributes)
        {
            string resolvedDisplayName = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add("for", forName);
            tag.SetInnerText(resolvedDisplayName);
            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), replaceExisting: true);
            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString LabelFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IEnumerable<TClass>>> expression1, Expression<Func<TClass, TProperty>> expression2)
        {
            return LabelFor(helper, expression1, expression2, null);
        }

        public static IHtmlString LabelFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IEnumerable<TClass>>> expression1, Expression<Func<TClass, TProperty>> expression2, object htmlAttributes)
        {
            return LabelFor(TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression2))), ModelMetadata.FromLambdaExpression(expression2, new ViewDataDictionary<TClass>()),
                                     ExpressionHelper.GetExpressionText(expression2), htmlAttributes);
        }
        
    }
}
