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
    public class HomeController : Controller
    {

        static int flag = 0;

        public ActionResult Index()
        {
            if (Session["rol"] != null)
            {
                List<molinos> molinosNormal = molinos.obtenerTodos();

                List<molinos> listaMolinosPreferencias = new List<molinos>();

                int j = 0;

                for (int i = 1; i < 7; i++) 
                {
                    if (Session["molino" + i] != null) 
                    {
                        molinos temp = new molinos().getMolino(Session["molino" + i].ToString());
                        while (listaMolinosPreferencias.Count+1 < i) 
                        {
                            if (!listaMolinosPreferencias.Contains(molinosNormal[j])) listaMolinosPreferencias.Add(molinosNormal[j]);
                            j++;
                        }
                        listaMolinosPreferencias.Add(temp);
                    }
                }

                for (int i = 0; i < molinosNormal.Count && listaMolinosPreferencias.Count<6; i++)
                {
                    if (!contieneDato(listaMolinosPreferencias, molinosNormal[i])) 
                    {
                        listaMolinosPreferencias.Add(molinosNormal[i]);
                    }
                }

                for (int i = 0; i < listaMolinosPreferencias.Count && i < 6; i++)
                {
                    string nombreMolino = listaMolinosPreferencias[i].nombre;
                    List<lecturas> listaLecturas = lecturas.obtenerUltimasLecturasMolino(nombreMolino);

                    if (i == 0)
                    {
                        ViewBag.nombreMolino1 = nombreMolino;
                        ViewBag.lecturasMolino1 = listaLecturas;
                        molinos molino = new molinos().getMolino(nombreMolino);
                        ViewBag.AlertaAlta1 = molino.alarmaAlta;
                        ViewBag.AlertaAltaMasTreinta1 = molino.alarmaAlta + 30;
                        ViewBag.AlertaMedia1 = molino.alarmaMedia;
                    }
                    else if (i == 1)
                    {
                        ViewBag.nombreMolino2 = nombreMolino;
                        ViewBag.lecturasMolino2 = listaLecturas;
                        molinos molino = new molinos().getMolino(nombreMolino);
                        ViewBag.AlertaAlta2 = molino.alarmaAlta;
                        ViewBag.AlertaAltaMasTreinta2 = molino.alarmaAlta + 30;
                        ViewBag.AlertaMedia2 = molino.alarmaMedia;
                    }
                    else if (i == 2)
                    {
                        ViewBag.nombreMolino3 = nombreMolino;
                        ViewBag.lecturasMolino3 = listaLecturas;
                        molinos molino = new molinos().getMolino(nombreMolino);
                        ViewBag.AlertaAlta3 = molino.alarmaAlta;
                        ViewBag.AlertaAltaMasTreinta3 = molino.alarmaAlta + 30;
                        ViewBag.AlertaMedia3 = molino.alarmaMedia;
                    }
                    else if (i == 3)
                    {
                        ViewBag.nombreMolino4 = nombreMolino;
                        ViewBag.lecturasMolino4 = listaLecturas;
                        molinos molino = new molinos().getMolino(nombreMolino);
                        ViewBag.AlertaAlta4 = molino.alarmaAlta;
                        ViewBag.AlertaAltaMasTreinta4 = molino.alarmaAlta + 30;
                        ViewBag.AlertaMedia4 = molino.alarmaMedia;
                    }
                    else if (i == 4)
                    {
                        ViewBag.nombreMolino5 = nombreMolino;
                        ViewBag.lecturasMolino5 = listaLecturas;
                        molinos molino = new molinos().getMolino(nombreMolino);
                        ViewBag.AlertaAlta5 = molino.alarmaAlta;
                        ViewBag.AlertaAltaMasTreinta5 = molino.alarmaAlta + 30;
                        ViewBag.AlertaMedia5 = molino.alarmaMedia;
                    }
                    else if (i == 5)
                    {
                        ViewBag.nombreMolino6 = nombreMolino;
                        ViewBag.lecturasMolino6 = listaLecturas;
                        molinos molino = new molinos().getMolino(nombreMolino);
                        ViewBag.AlertaAlta6 = molino.alarmaAlta;
                        ViewBag.AlertaAltaMasTreinta6 = molino.alarmaAlta + 30;
                        ViewBag.AlertaMedia6 = molino.alarmaMedia;
                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
            
        }

        public ActionResult Error()
        {
            return RedirectToAction("Error", "Shared");
            
        }

        private bool contieneDato(List<molinos> listaMolinosPreferencias, molinos molinos)
        {
            for (int i = 0; i < listaMolinosPreferencias.Count; i++) 
            {
                if (listaMolinosPreferencias[i].nombre.Equals(molinos.nombre)) return true;
            }
            return false;
        }

        public ActionResult login()
        {
            if (Session["rol"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (flag == 2) ViewBag.mensaje = "Contraseña cambiada";
                if (flag == 4) ViewBag.mensaje = "login erroneo";
                flag = 0;
                return View();
            }            
        }

        public ActionResult cambiarContraseña()
        {
            if (Session["rol"] != null)
            {
                if (flag == 1) ViewBag.error = "contraseñaIncorrecta";
                flag = 0;
                usuarios usuarioActual= usuarios.obtenerUsuario(Session["nombre"].ToString());
                return View(usuarioActual);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        public ActionResult seleccionarMolinosInicio() {
            if (Session["rol"] != null)
            {
                List<molinos> listaMolinos = molinos.obtenerTodos();
                return View(listaMolinos);
            }
            else 
            {
                return RedirectToAction("Index","Home");
            }

        }

        [HttpPost]
        public ActionResult verificarLogin(FormCollection post)
        {
            if (post["nombre"] == null || post["nombre"].ToString().Equals("")) 
            {
                Session.RemoveAll();
                return RedirectToAction("login", "Home");
            }
            else if (usuarios.revisarUsuarioPassword(post["nombre"], post["password"]))
            {
                Session["nombre"] = post["nombre"];
                Session["rol"] = usuarios.obtenerRol(post["nombre"]);
                //La sesión dura 12 horas por defecto
                Session.Timeout = 720;

                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.tipo = "Inicio de Sesión";
                nuevo.descripcion = "El usuario " + Session["nombre"] + " inició sesión en el sistema";
                nuevo.usuario = Session["nombre"].ToString();

                registros.agregarRegistro(nuevo);

                return RedirectToAction("Index", "Home");
            }

            flag = 4; //usuario y/o contraseña erróneos

            return RedirectToAction("Index");
        }

        public ActionResult cerrarSesion()
        {
                Session.RemoveAll();

                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult metodoCambioPassword(FormCollection post)
        {
            if (usuarios.revisarUsuarioPassword(post["nombreUsuario"], post["passwordActual"]))
                {
                    if (post["passwordNuevo"].ToString().Equals(post["passwordNuevo2"].ToString()))
                    {
                        usuarios.cambiarContraseña(post["nombreUsuario"].ToString(), post["passwordNuevo"].ToString());
                        flag = 2;//contraseña cambiada correctamente

                        registros nuevo = new registros();
                        nuevo.fecha = DateTime.Now;
                        nuevo.tipo = "Cambio de contraseña";
                        nuevo.usuario = Session["nombre"].ToString();
                        nuevo.descripcion = "El usuario " + nuevo.usuario + " ha modificado su contraseña";

                        registros.agregarRegistro(nuevo);

                        Session.RemoveAll();                       

                    }                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    flag = 1;//Contraseña incorrecta
                    return RedirectToAction("cambiarContraseña");
                }
        }

        public ActionResult aplicarSeleccionMolinos(FormCollection post)
        {
            if (Session["rol"] != null)
            {
                if (post["molino1"] != null && !post["molino1"].ToString().Equals("Seleccione una opción"))
                {
                    Session["molino1"] = post["molino1"].ToString();
                }
                if (post["molino2"] != null && !post["molino2"].ToString().Equals("Seleccione una opción"))
                {
                    Session["molino2"] = post["molino2"].ToString();
                }
                if (post["molino3"] != null && !post["molino3"].ToString().Equals("Seleccione una opción"))
                {
                    Session["molino3"] = post["molino3"].ToString();
                }
                if (post["molino4"] != null && !post["molino4"].ToString().Equals("Seleccione una opción"))
                {
                    Session["molino4"] = post["molino4"].ToString();
                }
                if (post["molino5"] != null && !post["molino5"].ToString().Equals("Seleccione una opción"))
                {
                    Session["molino5"] = post["molino5"].ToString();
                }
                if (post["molino6"] != null && !post["molino6"].ToString().Equals("Seleccione una opción"))
                {
                    Session["molino6"] = post["molino6"].ToString();
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }  

            if (usuarios.revisarUsuarioPassword(post["nombreUsuario"], post["passwordActual"]))
                {
                    if (post["passwordNuevo"].ToString().Equals(post["passwordNuevo2"].ToString()))
                    {
                        usuarios.cambiarContraseña(post["nombreUsuario"].ToString(), post["passwordNuevo"].ToString());
                        flag = 2;//contraseña cambiada correctamente

                        registros nuevo = new registros();
                        nuevo.fecha = DateTime.Now;
                        nuevo.tipo = "Cambio de contraseña";
                        nuevo.usuario = Session["nombre"].ToString();
                        nuevo.descripcion = "El usuario " + nuevo.usuario + " ha modificado su contraseña";

                        registros.agregarRegistro(nuevo);

                        Session.RemoveAll();                       

                    }                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    flag = 1;//Contraseña incorrecta
                    return RedirectToAction("cambiarContraseña");
                }
        }
    }
}
