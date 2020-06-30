using ComunidadDePracticaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ComunidadDePracticaMVC.Tests.Controllers
{
    public class CookieModel
    {
        public string Categoria { get; set; }
        public int Merito { get; set; }
        public int Peso { get; set; }
        public string Nombre { get; set; }

    }

    class CookieTestController
    {
        public static HttpCookie CreateCookieDummy(string rol) {
            CookieModel model = new CookieModel {
                Categoria = rol
            };
            var jsonUsuario = new JavaScriptSerializer().Serialize(model);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                "Nombre cookie",
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                true /*isPersistent*/,
                jsonUsuario /*userinfo*/,
                FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            
            return new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        }

        public class MockHttpContext : HttpContextBase
        {
            private readonly MockRequest request;
            public MockHttpContext(HttpCookieCollection cookies)
            {

                this.request = new MockRequest(cookies);
            }

            public override HttpRequestBase Request
            {
                get
                {
                    return request;
                }
            }

            public class MockRequest : HttpRequestBase
            {
                private readonly HttpCookieCollection cookies;
                public MockRequest(HttpCookieCollection cookies)
                {
                    this.cookies = cookies;
                }

                public override HttpCookieCollection Cookies
                {
                    get
                    {
                        return cookies;
                    }
                }
            }

        }

    }
}
