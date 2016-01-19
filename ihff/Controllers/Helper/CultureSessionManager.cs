using System.Globalization;
using System.Threading;
using System.Web.SessionState;

namespace ihff.Controllers.Helper
{
    public class CultureSessionManager
    {
        protected HttpSessionState session;

        public CultureSessionManager(HttpSessionState httpSessionState)
        {
            session = httpSessionState;
        }

        public static int CurrentCulture
        {
            get
            {
                // haal de huidige culture op
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    return 0;
                }
                if (Thread.CurrentThread.CurrentUICulture.Name == "nl-NL")
                {
                    return 1;

                }
                // default is 0 (engels)
                return 0;
            }
            set
            {
                // Zet de huidige Culture 0 = engels, 1 = nederlands
                if (value == 0)
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                else if (value == 1)
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-NL");
                else
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
        }
    }
}