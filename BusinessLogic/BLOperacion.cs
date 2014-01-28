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

       public List<BEOrdenInternamiento> Buscar(string strApellido, string strNombre)
        {
            try
            {
                return new DAOperacion().Buscar(strApellido, strNombre);
            }
            catch (Exception)
            {
                throw;
            }
        }

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
