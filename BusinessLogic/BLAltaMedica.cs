using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{
    public class BLAltaMedica
    {
        public List<BEAltaMedica> Buscar(string nombre)
        {
            try
            {
                return new DAAltaMedica().BuscarPaciente(nombre);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BEAltaMedica ObtenerDatosPaciente(int id)
        {
            try
            {
                return new DAAltaMedica().BuscarPaciente(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool RegistrarAltaMedica(BEAltaMedica BEA)
        {
            bool Resultado = false;
            try
            {
                Resultado = new DAAltaMedica().NuevaAltaMedica(BEA);
            }
            catch (Exception)
            {
                Resultado = false;
            }
            return Resultado;
        }

        public List<BEDoctor> ObtenerDoctores()
        {
            try
            {
                return new DAAltaMedica().ObtenerDoctores();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BEPacientes> ObtenerPacientes()
        {
            try
            {
                return new DAAltaMedica().ObtenerPacientes();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ActualizarAltaMedica(BEAltaMedica BEA)
        {
            bool Resultado = false;
            try
            {
                Resultado = new DAAltaMedica().ActualizarAltaMedica(BEA);
            }
            catch (Exception)
            {
                Resultado = false;
            }
            return Resultado;
        }

        public BEAltaMedica ObtenerDatosPaciente_Registro(int idOrdenInternamiento)
        {
            try
            {
                return new DAAltaMedica().BuscarPaciente_Registro(idOrdenInternamiento);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
