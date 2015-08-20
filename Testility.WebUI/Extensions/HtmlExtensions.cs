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

        internal static MvcHtmlString LabelFor(ModelMetadata metadata, string htmlFieldName)
        {
            string resolvedDisplayName = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            return new MvcHtmlString(HttpUtility.HtmlEncode(resolvedDisplayName));
        }

        public static IHtmlString LabelFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IEnumerable<TClass>>> expression1, Expression<Func<TClass, TProperty>> expression2)
        {
            return LabelFor(ModelMetadata.FromLambdaExpression(expression2, new ViewDataDictionary<TClass>()),
                                     ExpressionHelper.GetExpressionText(expression2));
        }
    }
}
