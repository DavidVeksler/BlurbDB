using System;
using System.Web.Mvc;

namespace E1Blurbs.Web.UI.Extensions
{
    public static class ControllerExtensions
    {
        internal static string GetRootControllerName(this HtmlHelper htmlHelper)
        {
            var viewContext = htmlHelper.ViewContext.IsChildAction
                ? htmlHelper.ViewContext.ParentActionViewContext
                : htmlHelper.ViewContext;
            return viewContext.RouteData.Values["controller"].ToString();
        }

        internal static string GetRootControllerName(this Controller controller)
        {
            var viewContext = controller.ControllerContext.IsChildAction
                ? controller.ControllerContext.ParentActionViewContext
                : controller.ControllerContext;
            return viewContext.RouteData.Values["controller"].ToString();
        }

        public static string GetCurrentTabCssClass(this HtmlHelper htmlHelper, string controllerName)
        {
            return IsCurrent(htmlHelper, controllerName) ? "active" : string.Empty;
        }

        public static bool IsCurrent(this HtmlHelper htmlHelper, string controllerName)
        {
            try
            {
                var curControllerName = GetRootControllerName(htmlHelper);
                return string.Equals(curControllerName, controllerName, StringComparison.InvariantCultureIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}