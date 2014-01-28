using BusinessEntities;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class OperacionController : Controller
    {

        public ActionResult Index()
        {
            List<BEOrdenInternamiento> lista = new List<BEOrdenInternamiento>();

            try
            {
                lista = new BLOperacion().Buscar("", "");

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
                lista = new BLOperacion().Buscar(strApellido, strNombre);

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(lista);
        }

        public ActionResult Generar(int id)
        {
            BEOrdenInternamiento objOI = new BEOrdenInternamiento();

            try
            {
                objOI = new BLOperacion().ObtenerOI(id);

                ViewBag.ListaProcedimiento = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Procedimiento), "IdParametro", "Nombre");
                ViewBag.ListaEntrada = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Entrada), "IdParametro", "Nombre");

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(objOI);
        }

        [HttpPost]
        public ActionResult Generar(BEOrdenInternamiento objOI, FormCollection form)
        {
            try
            {
                bool isOK = true;

                List<BEParametro> lstEntrada = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Entrada);

                ViewBag.ListaProcedimiento = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Procedimiento), "IdParametro", "Nombre");
                ViewBag.ListaEntrada = new SelectList(lstEntrada, "IdParametro", "Nombre");
                ViewBag.Mensaje = "";

                if (string.IsNullOrEmpty(form["Procedimiento"]))
                {
                    isOK = false;
                    ViewBag.Mensaje = "Debe seleccionar el tipo de procedimiento a realizar";
                }
                else
                {
                    foreach (BEParametro objP in lstEntrada)
                    {
                        if (string.IsNullOrEmpty(form["011o" + objP.IdParametro]))
                        {
                            isOK = false;
                            ViewBag.Mensaje = "Debe seleccionar la respuesta para la pregunta";
                            break;
                        }
                        else
                        {
                            if (form["011o" + objP.IdParametro].Equals("NO"))
                            {
                                if (string.IsNullOrEmpty(form["011t" + objP.IdParametro]))
                                {
                                    isOK = false;
                                    ViewBag.Mensaje = "Debe ingresar la descripcion para la respuesta";
                                    break;
                                }
                            }
                        }
                    }
                }

                if (isOK)
                {
                    BEProcedimiento objBE = new BEProcedimiento();
                    objBE.IdProcedimiento = 0;
                    objBE.TipoProcedimiento = form["Procedimiento"];
                    objBE.IdOrdenInternamiento = objOI.IdOrdenInternamiento;
                    objBE.Estado = "011";

                    foreach (BEParametro objP in lstEntrada)
                    {
                        BEProcedimientoPregunta objPP = new BEProcedimientoPregunta();
                        objPP.IdProcedimientoPregunta = 0;
                        objPP.IdProcedimiento = 0;
                        objPP.Tipo = "011";
                        objPP.Pregunta = objP.IdParametro;
                        objPP.Respuesta = form["011o" + objP.IdParametro];
                        objPP.Descripcion = form["011t" + objP.IdParametro];
                        objPP.Estado = "001";

                        objBE.Preguntas.Add(objPP);
                    }

                    if (new BLProcedimiento().Crear(objBE))
                    {
                        TempData["Mensaje"] = "Se genero....";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se registro el procedimiento";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(objOI);
        }

        public ActionResult Pausa(int id)
        {
            BEOrdenInternamiento objOI = new BEOrdenInternamiento();

            try
            {
                objOI = new BLOperacion().ObtenerOI(id);

                ViewBag.ListaPausaQuirurgica = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_PausaQuirurgica), "IdParametro", "Nombre");

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(objOI);
        }

        [HttpPost]
        public ActionResult Pausa(BEOrdenInternamiento objOI, FormCollection form)
        {
            try
            {
                bool isOK = true;

                List<BEParametro> lstPausa = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_PausaQuirurgica);

                ViewBag.ListaPausaQuirurgica = new SelectList(lstPausa, "IdParametro", "Nombre");
                ViewBag.Mensaje = "";

                foreach (BEParametro objP in lstPausa)
                {
                    if (string.IsNullOrEmpty(form["012o" + objP.IdParametro]))
                    {
                        isOK = false;
                        ViewBag.Mensaje = "Debe seleccionar la respuesta para la pregunta";
                        break;
                    }
                    else
                    {
                        if (form["012o" + objP.IdParametro].Equals("NO"))
                        {
                            if (string.IsNullOrEmpty(form["012t" + objP.IdParametro]))
                            {
                                isOK = false;
                                ViewBag.Mensaje = "Debe ingresar la descripcion para la respuesta";
                                break;
                            }
                        }
                    }
                }

                if (isOK)
                {
                    BEProcedimiento objBE = new BEProcedimiento();
                    objBE.IdProcedimiento = 0;
                    objBE.IdOrdenInternamiento = objOI.IdOrdenInternamiento;
                    objBE.Estado = "012";

                    foreach (BEParametro objP in lstPausa)
                    {
                        BEProcedimientoPregunta objPP = new BEProcedimientoPregunta();
                        objPP.IdProcedimientoPregunta = 0;
                        objPP.IdProcedimiento = 0;
                        objPP.Tipo = "012";
                        objPP.Pregunta = objP.IdParametro;
                        objPP.Respuesta = form["012o" + objP.IdParametro];
                        objPP.Descripcion = form["012t" + objP.IdParametro];
                        objPP.Estado = "001";

                        objBE.Preguntas.Add(objPP);
                    }

                    if (new BLProcedimiento().Actualizar(objBE))
                    {
                        TempData["Mensaje"] = "Se genero....";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se registro el procedimiento";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(objOI);
        }

        public ActionResult Salida(int id)
        {
            BEOrdenInternamiento objOI = new BEOrdenInternamiento();

            try
            {
                objOI = new BLOperacion().ObtenerOI(id);

                ViewBag.ListaSalida = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Salida), "IdParametro", "Nombre");

                ViewBag.Mensaje = "";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(objOI);
        }

        [HttpPost]
        public ActionResult Salida(BEOrdenInternamiento objOI, FormCollection form)
        {
            try
            {
                bool isOK = true;

                List<BEParametro> lstSalida = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Salida);

                ViewBag.ListaSalida = new SelectList(lstSalida, "IdParametro", "Nombre");
                ViewBag.Mensaje = "";

                foreach (BEParametro objP in lstSalida)
                {
                    if (string.IsNullOrEmpty(form["013o" + objP.IdParametro]))
                    {
                        isOK = false;
                        ViewBag.Mensaje = "Debe seleccionar la respuesta para la pregunta";
                        break;
                    }
                    else
                    {
                        if (form["013o" + objP.IdParametro].Equals("NO"))
                        {
                            if (string.IsNullOrEmpty(form["013t" + objP.IdParametro]))
                            {
                                isOK = false;
                                ViewBag.Mensaje = "Debe ingresar la descripcion para la respuesta";
                                break;
                            }
                        }
                    }
                }

                if (isOK)
                {
                    BEProcedimiento objBE = new BEProcedimiento();
                    objBE.IdProcedimiento = 0;
                    objBE.IdOrdenInternamiento = objOI.IdOrdenInternamiento;
                    objBE.Estado = "013";

                    foreach (BEParametro objP in lstSalida)
                    {
                        BEProcedimientoPregunta objPP = new BEProcedimientoPregunta();
                        objPP.IdProcedimientoPregunta = 0;
                        objPP.IdProcedimiento = 0;
                        objPP.Tipo = "013";
                        objPP.Pregunta = objP.IdParametro;
                        objPP.Respuesta = form["013o" + objP.IdParametro];
                        objPP.Descripcion = form["013t" + objP.IdParametro];
                        objPP.Estado = "001";

                        objBE.Preguntas.Add(objPP);
                    }

                    if (new BLProcedimiento().Actualizar(objBE))
                    {
                        TempData["Mensaje"] = "Se genero....";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se registro el procedimiento";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }

            return View(objOI);
        }

    }
}
