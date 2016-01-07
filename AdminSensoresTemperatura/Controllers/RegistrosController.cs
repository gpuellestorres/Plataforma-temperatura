using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;

namespace AdminSensoresTemperatura.Controllers
{
    public class RegistrosController : Controller
    {
        //
        // GET: /Registros/

        public ActionResult Index()
        {
            if (Session["rol"] != null)
            {
                List<registros> listaRegistros = registros.obtenerTodos();
                return View(listaRegistros);
            }
            else 
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}
