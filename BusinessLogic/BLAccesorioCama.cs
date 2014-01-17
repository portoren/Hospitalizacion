using System;
using System.Collections.Generic;

using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{

    public class BLAccesorioCama
    {

        public List<BEAccesorioCama> ObtenerAccesorioCamas()
        {
            try
            {
                return new DAAccesorioCama().ObtenerAccesorioCamas();
            }
            catch (Exception)
            {                
                throw;
            }
        }

    }

}
