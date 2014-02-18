using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using BusinessEntities;
using BusinessLogic;

namespace Clinica.Controllers
{
    
    public class OrdenInternamientoController : Controller
    {

        public ActionResult Index()
        {
            List<BEOrdenInternamiento> lista = new List<BEOrdenInternamiento>();

            try
            {
                lista = new BLOrdenInternamiento().Buscar("");

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
                string strPaciente = form["paciente"];
                lista = new BLOrdenInternamiento().Buscar(strPaciente);                

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(lista);
        }

        public ActionResult Asignar(int id)
        {
            BEOrdenInternamiento oi = new BEOrdenInternamiento();

            try
            {
                oi = new BLOrdenInternamiento().ObtenerPaciente(id);
                ViewBag.ListaHabitacion = new SelectList(new BLHabitacion().ObtenerDisponibles(), "IdHabitacion", "Nombre");
                ViewBag.ListaRecurso = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Recurso), "IdParametro", "Nombre");
                ViewBag.ListaCama = new SelectList(new BLCama().Disponible(), "IdCama", "Nombre");

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(oi);
        }

        [HttpPost]
        public ActionResult Asignar(BEOrdenInternamiento oi, string operacion = null, string Recurso = null, string Cantidad = null, string IdHabitacion = null, string IdCama = null)
        {
            if (oi == null)
                oi = new BEOrdenInternamiento();

            try
            {
                List<BEParametro> lstRecurso = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Recurso);
                ViewBag.ListaHabitacion = new SelectList(new BLHabitacion().ObtenerDisponibles(), "IdHabitacion", "Nombre");
                ViewBag.ListaRecurso = new SelectList(lstRecurso, "IdParametro", "Nombre");
                ViewBag.ListaCama = new SelectList(new BLCama().Disponible(), "IdCama", "Nombre");

                if (Session["lista"] != null)
                    oi.Recursos = (List <BEOrdenInternamientoRecurso>) Session["lista"];

                if (operacion == null)
                {
                    if (string.IsNullOrEmpty(IdHabitacion))
                        throw new ApplicationException("Debe seleccionar la habitacion");

                    if (string.IsNullOrEmpty(IdCama))
                        throw new ApplicationException("Debe seleccionar la cama");

                    if (oi.Recursos.Count < 1)
                        throw new ApplicationException("Debe agregar recursos a la habitacion");

                    oi.Estado = BEOrdenInternamiento.ESTADO_Asignado;

                    if (new BLOrdenInternamiento().Asignar(oi))
                    {
                        TempData["Mensaje"] = "Se actualizo correctamente la orden de internamiento";

                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.Mensaje = "No se puede asignar";
                }
                else if (operacion == "agregar-detalle")
                {
                    if (string.IsNullOrEmpty(Recurso))
                        throw new ApplicationException("Debe seleccionar el recurso");

                    string strRecurso = (from item in lstRecurso where item.IdParametro.Equals(Recurso) select item.Nombre).First();

                    if (string.IsNullOrEmpty(Cantidad))
                        throw new ApplicationException("Debe ingresar la cantidad");

                    int intCantidad = 0;
                    try
                    {
                        intCantidad = int.Parse(Cantidad);

                        if (intCantidad < 1)
                            throw new ApplicationException("La cantidad debe ser mayor o igual a 1");
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException("Debe ingresar la cantidad valida");
                    }

                    bool isExiste = false;
                    foreach(BEOrdenInternamientoRecurso recurso in oi.Recursos)
                    {
                        if (recurso.Recurso.Equals(Recurso))
                        {
                            recurso.Cantidad += intCantidad;
                            isExiste = true;

                            break;
                        }
                    }
                    if (!isExiste)
                        oi.Recursos.Add(new BEOrdenInternamientoRecurso(oi.IdOrdenInternamiento, intCantidad, Recurso, strRecurso));

                    Session["lista"] = oi.Recursos;
                }
                else if (operacion.StartsWith("eliminar-detalle-"))
                {
                    string indexStr = operacion.Replace("eliminar-detalle-", "");
                    int index = 0;

                    if (int.TryParse(indexStr, out index) && index >= 0 && index < oi.Recursos.Count)
                    {
                        var item = oi.Recursos.ToArray()[index];
                        oi.Recursos.Remove(item);
                        Session["lista"] = oi.Recursos;
                    }
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

        public JsonResult TipoHabitacion(int IdHabitacion)
        {
            string strTipoHabitacion = string.Empty;

            try
            {
                strTipoHabitacion = new BLHabitacion().ObtenerTipo(IdHabitacion);
            }
            catch (Exception)
            {                
            }

            return Json(strTipoHabitacion, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Editar(int id)
        {
            BEOrdenInternamiento oi = new BEOrdenInternamiento();

            try
            {
                oi = new BLOrdenInternamiento().ObtenerOI(id);
                Session["lista"] = oi.Recursos;

                ViewBag.ListaRecurso = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Recurso), "IdParametro", "Nombre");

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(oi);
        }

        [HttpPost]
        public ActionResult Editar(BEOrdenInternamiento oi, string operacion = null, string Recurso = null, string Cantidad = null)
        {
            if (oi == null)
                oi = new BEOrdenInternamiento();

            try
            {
                List<BEParametro> lstRecurso = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Recurso);
                ViewBag.ListaRecurso = new SelectList(lstRecurso, "IdParametro", "Nombre");

                if (Session["lista"] != null)
                    oi.Recursos = (List<BEOrdenInternamientoRecurso>)Session["lista"];

                if (operacion == null)
                {
                    int intTotal = (from items in oi.Recursos where items.Indicador == 0 select items).Count();

                    if (intTotal < 1)
                        throw new ApplicationException("Debe agregar recursos a la habitacion");

                    if (new BLOrdenInternamiento().Editar(oi))
                    {
                        TempData["Mensaje"] = "Se actualizo correctamente la orden de internamiento";

                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.Mensaje = "No se pudo editar";
                }
                else if (operacion == "agregar-detalle")
                {
                    if (string.IsNullOrEmpty(Recurso))
                        throw new ApplicationException("Debe seleccionar el recurso");

                    string strRecurso = (from item in lstRecurso where item.IdParametro.Equals(Recurso) select item.Nombre).First();

                    if (string.IsNullOrEmpty(Cantidad))
                        throw new ApplicationException("Debe ingresar la cantidad");

                    int intCantidad = 0;
                    try
                    {
                        intCantidad = int.Parse(Cantidad);

                        if (intCantidad < 1)
                            throw new ApplicationException("La cantidad debe ser mayor o igual a 1");
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException("Debe ingresar la cantidad valida");
                    }

                    bool isExiste = false;
                    foreach (BEOrdenInternamientoRecurso recurso in oi.Recursos)
                    {
                        if (recurso.Recurso.Equals(Recurso) && recurso.Indicador != 1)
                        {
                            recurso.Cantidad += intCantidad;
                            isExiste = true;

                            break;
                        }
                    }
                    if (!isExiste)
                        oi.Recursos.Add(new BEOrdenInternamientoRecurso(oi.IdOrdenInternamiento, intCantidad, Recurso, strRecurso));                    

                    Session["lista"] = oi.Recursos;
                }
                else if (operacion.StartsWith("eliminar-detalle-"))
                {
                    string indexStr = operacion.Replace("eliminar-detalle-", "");
                    int index = 0;

                    if (int.TryParse(indexStr, out index) && index >= 0 && index < oi.Recursos.Count)
                    {
                        var item = oi.Recursos.ToArray()[index];

                        if (item.IdOrdenInternamientoRecurso == 0)
                            oi.Recursos.Remove(item);
                        else
                            item.Indicador = 1;

                        Session["lista"] = oi.Recursos;
                    }
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

        public ActionResult Alta(int id)
        {
            BEOrdenInternamiento oi = new BEOrdenInternamiento();

            try
            {
                oi = new BLOrdenInternamiento().ObtenerOI(id);
                Session["lista"] = oi.Recursos;

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(oi);
        }

        [HttpPost]
        public ActionResult Alta(BEOrdenInternamiento oi, FormCollection form)
        {
            if (oi == null)
                oi = new BEOrdenInternamiento();

            try
            {
                if (Session["lista"] != null)
                    oi.Recursos = (List<BEOrdenInternamientoRecurso>)Session["lista"];

                int intTotal = (from items in oi.Recursos where items.Indicador == 0 select items).Count();

                if (intTotal > 0)
                {
                    string strIdOrdenInternamientoRecurso;
                    string strObservacion;

                    for (int i = 0; i < intTotal; i++)
                    {
                        strIdOrdenInternamientoRecurso = form["Detalle[" + i.ToString() + "].Id"];
                        strObservacion = form["Detalle[" + i.ToString() + "].Observacion"];

                        var item = (from recurso in oi.Recursos where recurso.IdOrdenInternamientoRecurso == int.Parse(strIdOrdenInternamientoRecurso) select recurso).First();
                        item.Observacion = strObservacion;
                    }
                }

                oi.Estado = BEOrdenInternamiento.ESTADO_Alta;
                if (new BLOrdenInternamiento().Alta(oi))
                {
                    TempData["Mensaje"] = "Se actualizo correctamente la orden de internamiento";

                    return RedirectToAction("Index");
                }
                else
                    ViewBag.Mensaje = "No se pudo dar de alta";
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
