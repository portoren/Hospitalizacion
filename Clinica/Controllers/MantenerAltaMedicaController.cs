using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntities;
using BusinessLogic;

namespace Clinica.Controllers
{
    public class MantenerAltaMedicaController : Controller
    {
        //
        // GET: /MantenerAltaMedica/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrarAltaMedica(string idOrdenInternamiento)
        {
            BEAltaMedica DatosAltaMedica = new BEAltaMedica();

            try
            {
                DatosAltaMedica = new BLAltaMedica().ObtenerDatosPaciente_Registro(int.Parse(idOrdenInternamiento));
                DatosAltaMedica.NumeroOrdenInternamiento = idOrdenInternamiento;
                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(DatosAltaMedica);
        }

        [HttpPost]
        public ActionResult RegistrarAltaMedica(FormCollection FormContext)
        {
            try
            {
                BEAltaMedica BEA = new BEAltaMedica();
                BEA.NumeroOrdenInternamiento = FormContext["NumeroOrdenInternamiento"];
                //BEA.idDoctor = int.Parse(FormContext["ListaDoctores"].ToString());
                //BEA.idPaciente = FormContext["ListaPacientes"];
                //BEA.idHabitacion = FormContext["ListaHabitaciones"];
                //BEA.idCama = int.Parse(FormContext["ListaTipoCama"].ToString());
                //BEA.Estado = FormContext["Estado"];
                BEA.Descripcion = FormContext["Descripcion"];
                
                bool Resultado = new BLAltaMedica().RegistrarAltaMedica(BEA);
                if (Resultado)
                {
                    ViewBag.Mensaje = "Se creo correctamente la cama";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Mensaje = "No se puede crear la Alta Medica";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View();
        }
        public ActionResult ActualizarAltaMedica(int id)
        {
            BEAltaMedica DatosAltaMedica = new BEAltaMedica();

            try
            {
                DatosAltaMedica = new BLAltaMedica().ObtenerDatosPaciente(id);
                IEnumerable<SelectListItem> Habitaciones = new SelectList(new BLHabitacion().ObtenerDisponibles(), "IdHabitacion", "Nombre", DatosAltaMedica.idHabitacion);
                ViewBag.ListaHabitaciones = Habitaciones;
                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(DatosAltaMedica);
        }

        [HttpPost]
        public ActionResult ActualizarAltaMedica(FormCollection FormContext)
        {
            BEAltaMedica BEA = new BEAltaMedica();

            BEA.IdOrdenInternamientoBitacora = int.Parse(FormContext["IdOrdenInternamientoBitacora"].ToString());
            //BEA.NombrePaciente = FormContext["NombrePaciente"].ToString();
            //BEA.ApellidoPaterno = FormContext["ApellidoPaterno"].ToString();
            //BEA.ApellidoMaterno = FormContext["ApellidoMaterno"].ToString();
            //BEA.idHabitacion = FormContext["ListaHabitaciones"];
            BEA.Descripcion = FormContext["Descripcion"];

            bool Resultado = new BLAltaMedica().ActualizarAltaMedica(BEA);
            if (Resultado)
            {
                ViewBag.Mensaje = "Se creo correctamente la cama";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = "No se puede crear la Alta Medica";
            }
            return View();
        }
        public ActionResult BuscarAltaMedica()
        {
            List<BEAltaMedica> ListaBusqueda = new List<BEAltaMedica>();
            return View(ListaBusqueda);
        }

        [HttpPost]
        public ActionResult BuscarAltaMedica(FormCollection form)
        {
            List<BEAltaMedica> ListaBusqueda = new List<BEAltaMedica>();
            try
            {
                string Nombre = form["nombrePaciente"];
                ListaBusqueda = new BLAltaMedica().Buscar(Nombre);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(ListaBusqueda);
        }
        public IView Resultado { get; set; }
    }
}
