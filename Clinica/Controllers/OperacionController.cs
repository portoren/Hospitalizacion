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

        public ActionResult Index(int intID)
        {
            BEOrdenInternamiento objOI = new BEOrdenInternamiento();

            try
            {
                objOI = new BLOperacion().ObtenerOI(intID);

                ViewBag.ListaProcedimiento = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Procedimiento), "IdParametro", "Nombre");
                ViewBag.ListaEntrada = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Entrada), "IdParametro", "Nombre");
                ViewBag.ListaPausaQuirurgica = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_PausaQuirurgica), "IdParametro", "Nombre");
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
        public ActionResult Index(BEOrdenInternamiento objOI, FormCollection form)
        {
            try
            {
                bool isOK = true;

                List<BEParametro> lstEntrada = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Entrada);
                List<BEParametro> lstPausa = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_PausaQuirurgica);
                List<BEParametro> lstSalida = new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Salida);

                ViewBag.ListaProcedimiento = new SelectList(new BLParametro().ObtenerParametros(BEParametro.DOMINIO_Procedimiento), "IdParametro", "Nombre");
                ViewBag.ListaEntrada = new SelectList(lstEntrada, "IdParametro", "Nombre");
                ViewBag.ListaPausaQuirurgica = new SelectList(lstPausa, "IdParametro", "Nombre");
                ViewBag.ListaSalida = new SelectList(lstSalida, "IdParametro", "Nombre");
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
                    foreach (BEParametro objP in lstPausa)
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
                    foreach (BEParametro objP in lstSalida)
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
                    objBE.Estado = "001";

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

                    foreach (BEParametro objP in lstPausa)
                    {
                        BEProcedimientoPregunta objPP = new BEProcedimientoPregunta();
                        objPP.IdProcedimientoPregunta = 0;
                        objPP.Tipo = "012";
                        objPP.Pregunta = objP.IdParametro;
                        objPP.Respuesta = form["012o" + objP.IdParametro];
                        objPP.Descripcion = form["012t" + objP.IdParametro];
                        objPP.Estado = "001";

                        objBE.Preguntas.Add(objPP);
                    }

                    foreach (BEParametro objP in lstSalida)
                    {
                        BEProcedimientoPregunta objPP = new BEProcedimientoPregunta();
                        objPP.IdProcedimientoPregunta = 0;
                        objPP.Tipo = "013";
                        objPP.Pregunta = objP.IdParametro;
                        objPP.Respuesta = form["013o" + objP.IdParametro];
                        objPP.Descripcion = form["013t" + objP.IdParametro];
                        objPP.Estado = "001";

                        objBE.Preguntas.Add(objPP);
                    }

                    if (new BLProcedimiento().Crear(objBE))
                    {
                        ViewBag.Mensaje = "Se registro el procedimiento";
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
