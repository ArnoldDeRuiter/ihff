using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;
using System.Web.Mvc;
using System.IO;

namespace ihff.Controllers
{
    public static class IViewExtensions
    {
        public static string GetWebFormViewName(this IView view)
        {
            if (view is WebFormView)
            {
                string viewUrl = ((WebFormView)view).ViewPath;
                string viewFileName = viewUrl.Substring(viewUrl.LastIndexOf('/'));
                string viewFileNameWithoutExtension = Path.GetFileNameWithoutExtension(viewFileName);
                return (viewFileNameWithoutExtension);
            }
            else
            {
                throw (new InvalidOperationException("This view is not a WebFormView"));
            }
        }
    }
}