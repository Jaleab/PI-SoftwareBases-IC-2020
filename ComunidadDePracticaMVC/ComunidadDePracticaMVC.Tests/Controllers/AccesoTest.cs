using ComunidadDePracticaMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ComunidadDePracticaMVC.Tests.Controllers
{
    [TestClass]
    public class AccesoTest
    {
        [TestMethod]
        public void EntrarVerUsuarios_Coordinador_Access() {

            //Arrange
            var cookies = new HttpCookieCollection();
            cookies.Add(CookieTestController.CreateCookieDummy("Coordinador"));

            var mockHttpContext = new CookieTestController.MockHttpContext(cookies);

            UsuarioController controller = new UsuarioController();
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act
            ViewResult resultado = controller.UsuariosComunidad("Mensaje") as ViewResult;

            //Assert 
            Assert.AreEqual("Se pudo entrar", resultado.ViewBag.Acceso);

        }
        [TestMethod]
        public void EntrarVerUsuarios_Nucleo_NoAccess() {
            //Arrange
            var cookies = new HttpCookieCollection();
            cookies.Add(CookieTestController.CreateCookieDummy("Núcleo"));

            var mockHttpContext = new CookieTestController.MockHttpContext(cookies);

            UsuarioController controller = new UsuarioController();
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act
            ViewResult resultado = controller.UsuariosComunidad("Mensaje") as ViewResult;

            //Assert 

            Assert.AreEqual(resultado, null);

        }

        [TestMethod]
        public void EntrarVerUsuarios_Periferico_NoAccess()
        {
            //Arrange
            var cookies = new HttpCookieCollection();
            cookies.Add(CookieTestController.CreateCookieDummy("Periférico"));

            var mockHttpContext = new CookieTestController.MockHttpContext(cookies);

            UsuarioController controller = new UsuarioController();
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act
            ViewResult resultado = controller.UsuariosComunidad("Mensaje") as ViewResult;

            //Assert 

            Assert.AreEqual(resultado, null);

        }

        [TestMethod]
        public void EntrarRevisarArticulos_Coordinador_Access() {
            //Arrange
            var cookies = new HttpCookieCollection();
            cookies.Add(CookieTestController.CreateCookieDummy("Coordinador"));

            var mockHttpContext = new CookieTestController.MockHttpContext(cookies);

            RevisionesController controller = new RevisionesController();
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act
            ViewResult resultado = controller.MisArticulosRevision() as ViewResult;

            //Assert 
            Assert.AreEqual("Se pudo entrar", resultado.ViewBag.Acceso);

        }
        [TestMethod]
        public void EntrarRevisarArticulos_Nucleo_Access() {
            //Arrange
            var cookies = new HttpCookieCollection();
            cookies.Add(CookieTestController.CreateCookieDummy("Núcleo"));

            var mockHttpContext = new CookieTestController.MockHttpContext(cookies);

            RevisionesController controller = new RevisionesController();
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act
            ViewResult resultado = controller.MisArticulosRevision() as ViewResult;

            //Assert 
            Assert.AreEqual("Se pudo entrar", resultado.ViewBag.Acceso);

        }
        [TestMethod]
        public void EntrarRevisarArticulos_Periferico_NoAccess() {
            //Arrange
            var cookies = new HttpCookieCollection();
            cookies.Add(CookieTestController.CreateCookieDummy("Periférico"));

            var mockHttpContext = new CookieTestController.MockHttpContext(cookies);

            RevisionesController controller = new RevisionesController();
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act
            ViewResult resultado = controller.MisArticulosRevision() as ViewResult;

            //Assert 
            Assert.AreEqual(resultado, null);


        }


    }
}
    