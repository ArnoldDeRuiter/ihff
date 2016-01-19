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
        {       }

        #endregion

        /// <summary>
        /// Manage the internationalization before to invokes the action in the current controller context.
        /// </summary>
        protected override void ExecuteCore()
        {

            // Controleer op ingestelde taal
            // Als de session Null is zet de culture op 0 (engels)
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

            // zet juiste culture (taal)
            CultureSessionManager.CurrentCulture = culture;

            // voor controller actie uit
            base.ExecuteCore();
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}