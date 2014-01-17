using System;
using System.Collections.Generic;

using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{

    public class BLCama
    {

        public List<BECama> Buscar(string strTipoCama, string strEstado)
        {
            try
            {
                return new DACama().Buscar(strTipoCama, strEstado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BECama Obtener(int intIdCama)
        {
            try
            {
                return new DACama().Obtener(intIdCama);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        public bool Nuevo(BECama objBE)
        {
            try
            {
                return new DACama().Nuevo(objBE);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Modificar(BECama objBE)
        {
            try
            {
                return new DACama().Modificar(objBE);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidarEliminar(int intIdCama, string strEstado)
        {
            try
            {
                int intExiste = new DACama().ValidarEliminar(intIdCama, strEstado);

                if (intExiste == 1)
                    return false;

                return true;
            }
            catch (Exception)
            {                
                throw;
            }
        }

        public bool Eliminar(BECama objBE)
        {
            try
            {
                return new DACama().Eliminar(objBE);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BECama> Disponible()
        {
            try
            {
                return new DACama().Disponible();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
