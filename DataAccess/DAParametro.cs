using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using BusinessEntities;

namespace DataAccess
{

    public class DAParametro : DABase
    {

        public List<BEParametro> ObtenerParametros(string strDominio)
        {
            try
            {
                List<BEParametro> lista = new List<BEParametro>();

                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select IdParametro, Nombre from Parametro where IdDominio = @v_Dominio"))
                {
                    this.db.AddInParameter(dbCmd, "@v_Dominio", DbType.String, strDominio);
                    
                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            lista.Add(new BEParametro(reader));
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
