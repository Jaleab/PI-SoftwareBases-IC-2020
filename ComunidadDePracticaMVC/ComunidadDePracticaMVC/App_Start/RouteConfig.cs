using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace ComunidadDePracticaMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Articulo",
                "Articulo/{action}/{hilera}",
                new { controller = "Articulo", action = "Index", hilera = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Usuario",
                "Usuario/{action}/{hilera}",
                new { controller = "Usuario", action = "GetNombreUsuarios", hilera = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ArticuloLargo",
                "ArticuloLargo/{action}/{hilera}",
                new { controller = "ArticuloLargo", action = "Index", hilera = UrlParameter.Optional }
            );
        }
    }
}
