using System;
using System.Collections.Generic;

using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{

    public class BLProcedimiento
    {

        public bool Crear(BEProcedimiento objBE)
        {
            try
            {
                return new DAProcedimiento().Crear(objBE);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
