using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using BusinessEntities;

namespace DataAccess
{

    public class DAAccesorioCama : DABase
    {

        public List<BEAccesorioCama> ObtenerAccesorioCamas()
        {
            try
            {
                List<BEAccesorioCama> lista = new List<BEAccesorioCama>();

                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select IdAccesorioCama, Nombre from AccesorioCama"))
                {
                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            lista.Add(new BEAccesorioCama(reader));
                    }
                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
