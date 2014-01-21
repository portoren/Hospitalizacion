using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using DataAccess;


namespace BusinessLogic
{
    public class BLOperacion
   {

       public BEOrdenInternamiento ObtenerOI(int intIdOrdenInternamiento)
       {
           try
           {
               return new DAOperacion().ObtenerOI(intIdOrdenInternamiento);
           }
           catch (Exception)
           {
               throw;
           }
       }

    }
}
