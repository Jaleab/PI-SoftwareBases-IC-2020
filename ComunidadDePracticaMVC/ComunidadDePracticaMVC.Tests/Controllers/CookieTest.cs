using System;
using ComunidadDePracticaMVC.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace ComunidadDePracticaMVC.Tests.Controllers
{
    [TestClass]
    public class CookieTest
    {
        //Test de coordinador
        [TestMethod]
        public void CookieTest_UserCoordinador_ReturnTrue() {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Coordinador");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Coordinador");
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CookieTest_UserCoordinador_ReturnFalse()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Coordinador");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Nucleo");
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void CookieTestVariousUsers_UserCoordinador_ReturnTrue()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Coordinador");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Coordinador,Nucleo,Prueba,Prueba2");
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CookieTestVariousUsers_UserCoordinador_ReturnFalse()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Coordinador");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Nucleo,Prueba,Prueba2,Periferico");
            Assert.IsFalse(resultado);
        }

        //Test de nucleo
        [TestMethod]
        public void CookieTest_UserNucleo_ReturnTrue()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Nucleo");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Nucleo");
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CookieTest_UserNucleo_ReturnFalse()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Nucleo");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Coordinador");
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void CookieTestVariousUsers_UserNucleo_ReturnTrue()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Nucleo");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Nucleo,Coordinador,Periferico,Prueba2");
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CookieTestVariousUsers_UserNucleo_ReturnFalse()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Nucleo");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Coordinador,Prueba,Prueba2,Periferico");
            Assert.IsFalse(resultado);
        }

        //Test de periferico
        [TestMethod]
        public void CookieTest_UserPeriferico_ReturnTrue()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Periferico");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Periferico");
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CookieTest_UserPeriferico_ReturnFalse()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Periferico");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Nucleo");
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void CookieTestVariousUsers_UserPeriferico_ReturnTrue()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Periferico");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Coordinador,Nucleo,Prueba,Periferico");
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CookieTestVariousUsers_UserPeriferico_ReturnFalse()
        {
            System.Web.HttpCookie auth = CookieTestController.CreateCookieDummy("Periferico");
            bool resultado = Services.CookieHandler.AuthorizeRole(auth, "Nucleo,Prueba,Prueba2,Coordinador");
            Assert.IsFalse(resultado);
        }
    }
}
