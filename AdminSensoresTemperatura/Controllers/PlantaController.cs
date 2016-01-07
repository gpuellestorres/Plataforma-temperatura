using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;
using System.Data.SqlClient;
using System.Data;

namespace AdminSensoresTemperatura.Controllers
{
    public class PlantaController : Controller
    {
        //
        // GET: /Planta/

        public static int flag = 0;

        public ActionResult Index()
        {
            if (Session["rol"] != null)
            {
                List<plantas> todas = plantas.obtenerTodas();

                if (flag == 1)
                {
                    ViewBag.mensaje = "se usa";
                }
                if (flag == 2)
                {
                    ViewBag.mensaje = "agregado";
                }
                if (flag == 3)
                {
                    ViewBag.mensaje = "eliminado";
                }
                if (flag == 4)
                {
                    ViewBag.mensaje = "modificado";
                }

                flag = 0;

                return View(todas);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Nueva()
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                List<string> listaOrganizaciones = new List<string>();
                List<organizaciones> todas = organizaciones.obtenerTodas();

                for (int i = 0; i < todas.Count; i++) 
                {
                    listaOrganizaciones.Add(todas[i].nombre);
                }

                ViewBag.Organizaciones = listaOrganizaciones;
                return View();
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult NuevaOrganizacion(string organizacion)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.organizacion = organizacion;
                List<string> listaOrganizaciones = new List<string>();
                List<organizaciones> todas = organizaciones.obtenerTodas();

                for (int i = 0; i < todas.Count; i++)
                {
                    listaOrganizaciones.Add(todas[i].nombre);
                }

                ViewBag.Organizaciones = listaOrganizaciones;
                return View("Nueva");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult NuevaIndex()
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                List<string> listaOrganizaciones = new List<string>();
                List<organizaciones> todas = organizaciones.obtenerTodas();

                for (int i = 0; i < todas.Count; i++) 
                {
                    listaOrganizaciones.Add(todas[i].nombre);
                }

                ViewBag.Organizaciones = listaOrganizaciones;
                ViewBag.desdeIndex = true;
                return View("Nueva");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Editar(string nombre)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                List<string> listaOrganizaciones = new List<string>();
                List<organizaciones> todas = organizaciones.obtenerTodas();

                for (int i = 0; i < todas.Count; i++)
                {
                    listaOrganizaciones.Add(todas[i].nombre);
                }

                ViewBag.Organizaciones = listaOrganizaciones;

                plantas planta = plantas.obtenerPlanta(nombre);

                return View(planta);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public string existePlanta(string nombre)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                string retorno = "false";

                if (plantas.existePlanta(nombre))
                {
                    retorno = "true";
                }

                return retorno;
            }
            return "";
        }

        public ActionResult agregarPlanta(FormCollection post)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                plantas nueva = new plantas();

                nueva.nombre = post["nombre"];
                nueva.organizacion = post["organizacion"];

                plantas.agregarBD(nueva);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario=Session["nombre"].ToString();
                nuevo.tipo = "Creación de planta";
                nuevo.descripcion = "El usuario " + nuevo.usuario + " ha creado una nueva Planta con nombre " + nueva.nombre + " en el sistema";

                registros.agregarRegistro(nuevo);

                flag = 2;//Agregado con éxito

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult editarPlanta(FormCollection post)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                plantas editada = plantas.obtenerPlanta(post["nombreAnterior"]);

                editada.nombre = post["nombre"];
                editada.organizacion = post["organizacion"];

                plantas.editarBD(editada, post["nombreAnterior"]);

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo = "Edición de planta";
                nuevo.descripcion = "El usuario " + nuevo.usuario 
                    + " ha editado los datos de la Planta con nombre " + editada.nombre + " del sistema";

                registros.agregarRegistro(nuevo);

                flag = 4;//Modificado con éxito

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult eliminar(string nombre)
        {
            if (Session["rol"] != null && (Session["rol"].ToString().Equals("analista") || Session["rol"].ToString().Equals("admin")))
            {
                if (!plantas.existePlantaEnMolinos(nombre))
                {
                    plantas.eliminarBD(nombre);

                    registros nuevo = new registros();
                    nuevo.fecha = DateTime.Now;
                    nuevo.usuario = Session["nombre"].ToString();
                    nuevo.tipo = "Eliminación de planta";
                    nuevo.descripcion = "El usuario " + nuevo.usuario
                        + " ha eliminado la Planta con nombre " + nombre + " del sistema";

                    registros.agregarRegistro(nuevo);

                    flag = 3;//Eliminado con éxito
                }
                else flag = 1;//No se puede eliminar porque se está ocupando

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

    }
}
