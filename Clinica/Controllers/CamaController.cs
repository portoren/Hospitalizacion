using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BusinessEntities;
using BusinessLogic;

namespace Clinica.Controllers
{

    public class CamaController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"];
            
            return View();
        }

        public ActionResult Buscar(int tipo)
        {
            List<BECama> lista = new List<BECama>();

            try
            {                
                ViewBag.ListaTipoCama = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_TipoCama), "IdParametro", "Nombre");
                ViewBag.ListaEstadoCama = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_EstadoCama), "IdParametro", "Nombre");

                ViewBag.Modificar = false;
                ViewBag.Eliminar = true;
            
                if (tipo == 0)
                {
                    ViewBag.Modificar = true;
                    ViewBag.Eliminar = false;
                }

                ViewBag.Mensaje = TempData["Mensaje"];
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }
            
            return View(lista);
        }

        [HttpPost]
        public ActionResult Buscar(int tipo, FormCollection form)
        {
            List<BECama> lista = new List<BECama>();

            try
            {
                string strTipoCama = form["paciente"];
                string strEstadoServicio = form["EstadoServicio"];

                lista = new BLCama().Buscar(strTipoCama, strEstadoServicio);

                ViewBag.ListaTipoCama = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_TipoCama), "IdParametro", "Nombre");
                ViewBag.ListaEstadoCama = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_EstadoCama), "IdParametro", "Nombre");

                ViewBag.Modificar = false;
                ViewBag.Eliminar = true;

                if (tipo == 0)
                {
                    ViewBag.Modificar = true;
                    ViewBag.Eliminar = false;
                }

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(lista);
        }

        public ActionResult Nuevo()
        {
            var cama = new BECama();

            try
            {
                ViewBag.ListaMarca = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Marca), "IdParametro", "Nombre");
                ViewBag.ListaTipoCama = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_TipoCama), "IdParametro", "Nombre");
                ViewBag.ListaModoOperacion = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_ModoOperacion), "IdParametro", "Nombre");
                ViewBag.ListaTipoColchon = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_TipoColchon), "IdParametro", "Nombre");
                
                cama.Accesorios = new BLAccesorioCama().ObtenerAccesorioCamas();

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(cama);
        }

        [HttpPost]
        public ActionResult Nuevo(BECama cama)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cama.Estado = BECama.ESTADO_Abierta;

                    if (new BLCama().Nuevo(cama))
                    {
                        TempData["Mensaje"] = "Se creo correctamente la cama";

                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.Mensaje = "No se puede crear";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(cama);
        }

        public ActionResult Editar(int id)
        {
            var cama = new BECama();

            try
            {
                ViewBag.ListaMarca = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Marca), "IdParametro", "Nombre");
                ViewBag.ListaTipoCama = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_TipoCama), "IdParametro", "Nombre");
                ViewBag.ListaModoOperacion = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_ModoOperacion), "IdParametro", "Nombre");
                ViewBag.ListaTipoColchon = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_TipoColchon), "IdParametro", "Nombre");

                cama = new BLCama().Obtener(id);

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(cama);
        }

        [HttpPost]
        public ActionResult Editar(BECama cama)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (new BLCama().Modificar(cama))
                    {
                        TempData["Mensaje"] = "Se actualizo correctamente la cama";

                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.Mensaje = "No se puede modificar";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(cama);
        }

        public ActionResult Borrar(int id)
        {
            try
            {
                bool eliminar = new BLCama().ValidarEliminar(id, BECama.ESTADO_EnUso);

                if (eliminar)
                {
                    if (new BLCama().Eliminar(new BECama { IdCama = id, Estado = BECama.ESTADO_Anulada }))
                    {
                        TempData["Mensaje"] = "Se elimino correctamente la cama";

                        return RedirectToAction("Index");
                    }
                    else
                        TempData["Mensaje"] = "No se puede eliminar";
                }
                else
                    TempData["Mensaje"] = "No se puede eliminar porque esta en uso";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
            }
            
            return RedirectToAction("Buscar", new { tipo = 1 });
        }

    }

}
