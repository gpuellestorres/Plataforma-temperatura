using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminSensoresTemperatura.Models;

namespace AdminSensoresTemperatura.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult formularioReporte(string EPC, string inicio, string fin) 
        {
            if (Session["rol"] != null)
            {
                ViewBag.EPC = EPC;
                ViewBag.Inicio = inicio;
                ViewBag.Fin = fin;
                ViewBag.Sensor = new sensores().getSensor(EPC).numero;
                ViewBag.Molino = new sensores().getSensor(EPC).molino;

                return View();
            }
            else {
                return RedirectToAction("login", "Home");
            }
        }

        public FileContentResult obtenerReporte(FormCollection post)
        {
            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo

            string EPC = post["EPC"].ToString();
            string inicio = post["inicio"].ToString();
            string fin = post["fin"].ToString();

            string diagnostico = post["diagnostico"];
            string recomendacion = post["recomendacion"];
            string observaciones = post["observaciones"];

            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo
            LocalReport reporte_local = new LocalReport();
            // pasa la ruta donde se encuentra el reporte
            reporte_local.ReportPath = Server.MapPath("~/Report/reporte.rdlc");
            // creamos un recurso de datos del tipo report
            ReportDataSource conjunto_datos = new ReportDataSource();
            // le asginamos al conjuto de datos el nombre del datasource del reporte
            conjunto_datos.Name = "DataSet1";
            List<lecturasReporte> lecturasParaReporte = new List<lecturasReporte>();
            if (Session["rol"] != null)
            {
                List<lecturas> listaLecturas = lecturas.obtenerUltimosDatos(EPC, inicio, fin);
                lecturasParaReporte = lecturasReporte.convertirLecturas(listaLecturas, diagnostico, recomendacion, observaciones);
            }
            // se le asigna el datasource el conjunto de datos desde el modelo
            conjunto_datos.Value = lecturasParaReporte;
            // se agrega el conjunto de datos del tipo report al reporte local
            reporte_local.DataSources.Add(conjunto_datos);
            // datos para renderizar como se mostrara el reporte
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>14in</PageWidth>" +
                 "  <PageHeight>12in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Se renderiza el reporte            
            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // el reporte es mostrado como una imagen
            return File(renderedBytes, mimeType);
        }
    }
}
