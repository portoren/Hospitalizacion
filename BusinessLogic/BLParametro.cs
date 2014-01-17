using System;
using System.Collections.Generic;

using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{

    public class BLParametro
    {

        public List<BEParametro> ObtenerParametros(string strDominio) 
        {
            try
            {
                return new DAParametro().ObtenerParametros(strDominio);
            }
            catch (Exception)
            {                
                throw;
            }
        }

    }

}
