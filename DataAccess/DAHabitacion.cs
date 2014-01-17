using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using BusinessEntities;

namespace DataAccess
{
    
    public class DAHabitacion: DABase
    {

        public List<BEHabitacion> ObtenerDisponibles()
        {
            try
            {
                List<BEHabitacion> lista = new List<BEHabitacion>();

                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select IdHabitacion, 'Numero ' + Numero as Numero from Habitacion where Disponible = 0"))
                {
                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            lista.Add(new BEHabitacion(reader));
                    }

                    return lista;
                }
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
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select (select top 1 Nombre from Parametro where IdDominio = '006' and IdParametro = Tipo) from Habitacion where IdHabitacion = @n_idhabitacion"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idhabitacion", DbType.Int32, intIdHabitacion);

                    return this.db.ExecuteScalar(dbCmd).ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }

}
