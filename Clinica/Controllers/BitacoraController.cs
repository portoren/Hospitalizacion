using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntities;
using BusinessLogic;

namespace Clinica.Controllers
{
    public class BitacoraController : Controller
    {

        public ActionResult Index()
        {
            List<BEOrdenInternamiento> lista = new List<BEOrdenInternamiento>();

            try
            {
                lista = new BLBitacora().Buscar("", "");

                ViewBag.Mensaje = TempData["Mensaje"];
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(lista);           
        }


        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            List<BEOrdenInternamiento> lista = new List<BEOrdenInternamiento>();

            try
            {
                string strApellido = form["paciente"];
                string strNombre = form["pacientenombre"];
                lista = new BLBitacora().Buscar(strApellido, strNombre);

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(lista);
        }

        public ActionResult Actualizar(int id)
        {
            BEOrdenInternamiento bitacora = new BEOrdenInternamiento();

            try
            {
                bitacora = new BLBitacora().ObtenerBitacora(id);
                Session["lista"] = bitacora.Bitacora;

                ViewBag.ListaEstadoPaciente = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_EstadoPaciente), "IdParametro", "Nombre");

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(bitacora);
        }

        public ActionResult Ver(int id)
        {
            BEOrdenInternamiento bitacora = new BEOrdenInternamiento();

            try
            {
                bitacora = new BLBitacora().ObtenerBitacora(id);

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(bitacora);
        }

        [HttpPost]
        public ActionResult Actualizar(BEOrdenInternamiento oi, string operacion = null, string EstadoPaciente = null, string Fecha = null, string Descripcion = null)
        {
            if (oi == null)
                oi = new BEOrdenInternamiento();

            try
            {
                List<BEParametro> lstEP = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_EstadoPaciente);
                ViewBag.ListaEstadoPaciente = new SelectList(lstEP, "IdParametro", "Nombre");

                if (Session["lista"] != null)
                    oi.Bitacora = (List<BEOrdenInternamientoBitacora>)Session["lista"];

                if (operacion == null)
                {
                    if (string.IsNullOrEmpty(EstadoPaciente))
                        throw new ApplicationException("Debe seleccionar el Estado del Paciente");

                    string strEstadoPacienteNombre = (from item in lstEP where item.IdParametro.Equals(EstadoPaciente) select item.Nombre).First();

                    if (string.IsNullOrEmpty(Fecha))
                        throw new ApplicationException("Debe ingresar la Fecha");

                    Fecha = Fecha.Replace("T"," ");

                    DateTime dtFecha;
                    if (!(DateTime.TryParseExact(Fecha, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtFecha)))
                        throw new ApplicationException("Debe ingresar la Fecha válida");
                    
                    if (string.IsNullOrEmpty(Descripcion))
                        throw new ApplicationException("Debe ingresar la Descripción");

                    BEOrdenInternamientoBitacora beOIB = new BEOrdenInternamientoBitacora();
                    beOIB.IdOrdenInternamientoBitacora = 0;
                    beOIB.IdOrdenInternamiento = oi.IdOrdenInternamiento;
                    beOIB.Fecha = dtFecha;
                    beOIB.EstadoPaciente = EstadoPaciente;
                    beOIB.EstadoPacienteNombre = strEstadoPacienteNombre;
                    beOIB.Descripcion = Descripcion;
                    beOIB.Estado = BEOrdenInternamientoBitacora.ESTADO_Activo;

                    if (new BLBitacora().Crear(beOIB))
                    {
                        oi.Bitacora.Add(beOIB);
                        Session["lista"] = oi.Bitacora;

                        ViewBag.Mensaje = "Se agrego correctamente la bitacora al orden de internamiento";
                    }
                    else
                        ViewBag.Mensaje = "No se puede asignar";
                }
                else if (operacion.StartsWith("eliminar-detalle-"))
                {
                    string indexStr = operacion.Replace("eliminar-detalle-", "");

                    if (new BLBitacora().Eliminar(int.Parse(indexStr)))
                    {
                        BEOrdenInternamientoBitacora objBE = (from item in oi.Bitacora where item.IdOrdenInternamientoBitacora == int.Parse(indexStr) select item).First();

                        oi.Bitacora.Remove(objBE);
                        Session["lista"] = oi.Bitacora;

                        ViewBag.Mensaje = "Se elimino correctamente la bitacora al orden de internamiento";
                    }
                    else
                        ViewBag.Mensaje = "No se puede eliminar";
                }
            }
            catch (ApplicationException ex)
            {
                ViewBag.Mensaje = ex.Message;
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(oi);
        }

    }
}
