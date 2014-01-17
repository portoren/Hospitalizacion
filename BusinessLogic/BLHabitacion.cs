using System;
using System.Collections.Generic;

using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{

    public class BLHabitacion
    {

        public List<BEHabitacion> ObtenerDisponibles()
        {
            try
            {
                return new DAHabitacion().ObtenerDisponibles();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ObtenerTipo(int intIdHabitacion)
        {
            try
            {
                return new DAHabitacion().ObtenerTipo(intIdHabitacion);
            }
            catch (Exception)
            {                
                throw;
            }
        }

    }

}
