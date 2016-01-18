using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ihff.Controllers.Helper;

namespace ihff.Controllers
{
    public class BaseController : Controller
    {
        #region OnActionExecuting

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //ActionExecutingContext abc = filterContext;
            //SessionManager _sm = new SessionManager();
            //if (_sm.IsNull == true || _sm.User_Id == 0)
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" }, { "returnUrl", HttpContext.Request.RawUrl }, { "area", "" } });
            //    return;
            //}

            //string controllerName = filterContext.Controller.GetType().Name.Replace("Controller", "");
            //string actionMethodName = filterContext.ActionDescriptor.ActionName;

            //if (_sm.System_Id != 1)
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //     new RouteValueDictionary { { "controller", "Home" }, { "action", "Index" }, { "area", "" } });
            //    return;
            //}
        }

        #endregion

        /// <summary>
        /// Manage the internationalization before to invokes the action in the current controller context.
        /// </summary>
        protected override void ExecuteCore()
        {
            int culture = 0;
            if (this.Session["CurrentCulture"] == null)
            {
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
                this.Session["CurrentCulture"] = culture;
            }
            else
            {
                culture = (int)this.Session["CurrentCulture"];
            }
            //
            CultureSessionManager.CurrentCulture = culture;
            //
            // Invokes the action in the current controller context.
            //
            base.ExecuteCore();
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}