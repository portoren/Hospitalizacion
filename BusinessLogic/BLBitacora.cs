using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using DataAccess;


namespace BusinessLogic
{
   public class BLBitacora
   {

       public List<BEOrdenInternamiento> Buscar(string strApellido, string strNombre)
       {
           try
           {
               return new DABitacora().Buscar(strApellido, strNombre);
           }
           catch (Exception)
           {
               throw;
           }
       }

       public bool Crear(BEOrdenInternamientoBitacora beOIB)
       {
           try
           {
               return new DABitacora().Crear(beOIB);
           }
           catch (Exception)
           {
               throw;
           }
       }

       public BEOrdenInternamiento ObtenerBitacora(int intIdOrdenInternamiento)
       {
           try
           {
               return new DABitacora().ObtenerBitacora(intIdOrdenInternamiento);
           }
           catch (Exception)
           {
               throw;
           }
       }

       public bool Eliminar(int intID)
       {
           try
           {
               return new DABitacora().Eliminar(intID);
           }
           catch (Exception)
           {
               throw;
           }
       }

    }
}
