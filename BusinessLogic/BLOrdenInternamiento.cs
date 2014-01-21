using System;
using System.Collections.Generic;

using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{

    public class BLOrdenInternamiento
    {

        public List<BEOrdenInternamiento> Buscar(string strPaciente)
        {
            try
            {
                return new DAOrdenInternamiento().Buscar(strPaciente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BEOrdenInternamiento ObtenerPaciente(int intIdOrdenInternamiento)
        {
            try
            {
                return new DAOrdenInternamiento().ObtenerPaciente(intIdOrdenInternamiento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Asignar(BEOrdenInternamiento objBE)
        {
            try
            {
                return new DAOrdenInternamiento().Asignar(objBE);
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
                return new DAOrdenInternamiento().ObtenerOI(intIdOrdenInternamiento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Editar(BEOrdenInternamiento objBE)
        {
            try
            {
                return new DAOrdenInternamiento().Editar(objBE);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Alta(BEOrdenInternamiento objBE)
        {
            try
            {
                return new DAOrdenInternamiento().Alta(objBE);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ValidarEstado(int intId, string strEstado)
        {
            try
            {
                return new DAOrdenInternamiento().ValidarEstado(intId, strEstado);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
