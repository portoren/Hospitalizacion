using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using BusinessEntities;

namespace DataAccess
{
    
    public class DACama: DABase
    {

        public List<BECama> Buscar(string strTipoCama, string strEstado)
        {
            try
            {
                List<BECama> lista = new List<BECama>();

                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select c.IdCama, (select top 1 Nombre from parametro where IdDominio = @v_dominiomarca and IdParametro = c.Marca) as Marca, (select top 1 Nombre from parametro where IdDominio = @v_dominiotipocama and IdParametro = c.TipoCama) as TipoCama, (select top 1 Nombre from parametro where IdDominio = @v_dominiomodooperacion and IdParametro = c.ModoOperacion) as ModoOperacion, (select top 1 Nombre from parametro where IdDominio = @v_dominiotipocolchon and IdParametro = c.TipoColchon) as TipoColchon, ltrim(rtrim(isnull(stuff((select '; ' + ac.Nombre from Cama_AccesorioCama cac inner join AccesorioCama ac on ac.IdAccesorioCama = cac.IdAccesorioCama and cac.IdCama = c.IdCama for xml path('')), 1, 1, ''),''))) as Accesorios, (select top 1 Nombre from parametro where IdDominio = @v_dominioestado and IdParametro = c.Estado) as Estado from Cama c where (c.TipoCama = @v_tipocama or @v_tipocama is null) and (c.Estado = @v_estado or @v_estado is null)"))
                {
                    this.db.AddInParameter(dbCmd, "@v_dominiomarca", DbType.String, BEParametro.DOMINIO_Marca);
                    this.db.AddInParameter(dbCmd, "@v_dominiotipocama", DbType.String, BEParametro.DOMINIO_TipoCama);
                    this.db.AddInParameter(dbCmd, "@v_dominiomodooperacion", DbType.String, BEParametro.DOMINIO_ModoOperacion);
                    this.db.AddInParameter(dbCmd, "@v_dominiotipocolchon", DbType.String, BEParametro.DOMINIO_TipoColchon);
                    this.db.AddInParameter(dbCmd, "@v_dominioestado", DbType.String, BEParametro.DOMINIO_EstadoCama);
                    
                    if (string.IsNullOrEmpty(strTipoCama))
                        this.db.AddInParameter(dbCmd, "@v_tipocama", DbType.String, DBNull.Value);
                    else
                        this.db.AddInParameter(dbCmd, "@v_tipocama", DbType.String, strTipoCama);

                    if (string.IsNullOrEmpty(strEstado))
                        this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, DBNull.Value);
                    else
                        this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, strEstado);

                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            lista.Add(new BECama(reader, 1));
                    }

                    return lista;
                }
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
                BECama objBE = null;

                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select IdCama, Marca, Modelo, TipoCama, ModoOperacion, TipoColchon, Estado from Cama where IdCama = @n_idcama"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idcama", DbType.Int32, intIdCama);
                    
                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                        {
                            objBE = new BECama(reader, 0);

                            break;
                        }
                    }
                }

                if (objBE == null)
                    objBE = new BECama();

                objBE.Accesorios = new DAAccesorioCama().ObtenerAccesorioCamas();

                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select IdAccesorioCama from Cama_AccesorioCama where IdCama = @n_idcama"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idcama", DbType.Int32, intIdCama);

                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                        {
                            var accesoriocama = (from row in objBE.Accesorios
                                                 where row.IdAccesorioCama == Convert.ToInt32(reader["IdAccesorioCama"])
                                                 select row).First();

                            accesoriocama.Marcado = true;
                        }
                    }
                }

                return objBE;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ValidarEliminar(int intIdCama, string strEstado)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select count(1) from Cama where IdCama = @n_IdCama and Estado = @v_Estado"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idcama", DbType.Int32, intIdCama);
                    this.db.AddInParameter(dbCmd, "@v_Estado", DbType.String, strEstado);

                    return (int)this.db.ExecuteScalar(dbCmd);
                }
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
                List<BECama> lista = new List<BECama>();

                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select IdCama, 'Cama ' + cast(IdCama as varchar(100)) + ' - Modelo: ' + Modelo as Marca from Cama where Estado = '001'"))
                {
                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            lista.Add(new BECama(reader, 2));
                    }

                    return lista;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ExisteAccesorio(DbTransaction dbTr, int intIdCama, int intIdAccesorioCama)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select count(1) from Cama_AccesorioCama where IdCama = @n_IdCama and IdAccesorioCama = @n_IdAccesorioCama"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idcama", DbType.Int32, intIdCama);
                    this.db.AddInParameter(dbCmd, "@n_IdAccesorioCama", DbType.Int32, intIdAccesorioCama);

                    return (int)this.db.ExecuteScalar(dbCmd, dbTr);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Nuevo(BECama objBE)
        {
            bool isOK = false;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    isOK = this.Insertar(dbTr, objBE);

                    if (isOK)
                    {
                        int intFilas = (from row in objBE.Accesorios 
                                        where row.Marcado == true 
                                        select row).Count();

                        if (intFilas > 0)
                            isOK = this.InsertarAccesorios(dbTr, objBE.Accesorios, objBE.IdCama);
                    }
                    
                    if (isOK)
                        dbTr.Commit();
                    else
                        dbTr.Rollback();
                }
                catch (Exception)
                {
                    dbTr.Rollback();

                    throw;
                }
                finally
                {
                    if (dbCn.State == ConnectionState.Open)
                        dbCn.Close();
                }

                return isOK;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        private bool Insertar(DbTransaction dbTr, BECama objBE)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("pa_cama_set_insert"))
                {
                    this.db.AddOutParameter(dbCmd, "@n_id", DbType.Int32, objBE.IdCama);
                    this.db.AddInParameter(dbCmd, "@v_marca", DbType.String, objBE.Marca);
                    this.db.AddInParameter(dbCmd, "@v_modelo", DbType.String, objBE.Modelo);
                    this.db.AddInParameter(dbCmd, "@v_tipocama", DbType.String, objBE.TipoCama);
                    this.db.AddInParameter(dbCmd, "@v_modooperacion", DbType.String, objBE.ModoOperacion);
                    this.db.AddInParameter(dbCmd, "@v_tipocolchon", DbType.String, objBE.TipoColchon);
                    this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, objBE.Estado);

                    if (this.db.ExecuteNonQuery(dbCmd, dbTr) > 0)
                    {
                        objBE.IdCama = (int) this.db.GetParameterValue(dbCmd, "@n_id");

                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Modificar(BECama objBE)
        {
            bool isOK = false;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    isOK = this.Modificar(dbTr, objBE);

                    if (isOK)
                        isOK = this.ActualizarAccesorios(dbTr, objBE.Accesorios, objBE.IdCama);
                    
                    if (isOK)
                        dbTr.Commit();
                    else
                        dbTr.Rollback();
                }
                catch (Exception)
                {
                    dbTr.Rollback();

                    throw;
                }
                finally
                {
                    if (dbCn.State == ConnectionState.Open)
                        dbCn.Close();
                }

                return isOK;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool Modificar(DbTransaction dbTr, BECama objBE)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update Cama set Marca = @v_marca, Modelo = @v_modelo, TipoCama = @v_tipocama, ModoOperacion = @v_modooperacion, TipoColchon = @v_tipocolchon, Estado = @v_estado where IdCama = @n_id"))
                {
                    this.db.AddInParameter(dbCmd, "@v_marca", DbType.String, objBE.Marca);
                    this.db.AddInParameter(dbCmd, "@v_modelo", DbType.String, objBE.Modelo);
                    this.db.AddInParameter(dbCmd, "@v_tipocama", DbType.String, objBE.TipoCama);
                    this.db.AddInParameter(dbCmd, "@v_modooperacion", DbType.String, objBE.ModoOperacion);
                    this.db.AddInParameter(dbCmd, "@v_tipocolchon", DbType.String, objBE.TipoColchon);
                    this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, objBE.Estado);
                    this.db.AddInParameter(dbCmd, "@n_id", DbType.Int32, objBE.IdCama);

                    if (this.db.ExecuteNonQuery(dbCmd, dbTr) > 0)
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Eliminar(BECama objBE)
        {
            bool isOK = false;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    isOK = this.Eliminar(dbTr, objBE);

                    if (isOK)
                        dbTr.Commit();
                    else
                        dbTr.Rollback();
                }
                catch (Exception)
                {
                    dbTr.Rollback();

                    throw;
                }
                finally
                {
                    if (dbCn.State == ConnectionState.Open)
                        dbCn.Close();
                }

                return isOK;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool Eliminar(DbTransaction dbTr, BECama objBE)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update Cama set Estado = @v_estado where IdCama = @n_id"))
                {
                    this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, objBE.Estado);
                    this.db.AddInParameter(dbCmd, "@n_id", DbType.Int32, objBE.IdCama);

                    if (this.db.ExecuteNonQuery(dbCmd, dbTr) > 0)
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool InsertarAccesorios(DbTransaction dbTr, List<BEAccesorioCama> lstAccesorioCama, int intIdCama)
        {
            try
            {
                var lstBE = (from row in lstAccesorioCama
                             where row.Marcado == true
                             select row);

                foreach (BEAccesorioCama objBE in lstBE)
                {
                    using (DbCommand dbCmd = this.db.GetSqlStringCommand("insert into Cama_AccesorioCama (IdAccesorioCama, IdCama) values (@n_IdAccesorioCama, @n_IdCama)"))
                    {
                        this.db.AddInParameter(dbCmd, "@n_IdAccesorioCama", DbType.Int32, objBE.IdAccesorioCama);
                        this.db.AddInParameter(dbCmd, "@n_IdCama", DbType.Int32, intIdCama);

                        if (this.db.ExecuteNonQuery(dbCmd, dbTr) < 1)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ActualizarAccesorios(DbTransaction dbTr, List<BEAccesorioCama> lstAccesorioCama, int intIdCama)
        {            
            try
            {
                int intExiste;

                foreach (BEAccesorioCama objBE in lstAccesorioCama)
                {
                    intExiste = this.ExisteAccesorio(dbTr, intIdCama, objBE.IdAccesorioCama);
                    
                    if (intExiste == 0 && objBE.Marcado)
                        using (DbCommand dbCmd = this.db.GetSqlStringCommand("insert into Cama_AccesorioCama (IdAccesorioCama, IdCama) values (@n_IdAccesorioCama, @n_IdCama)"))
                        {
                            this.db.AddInParameter(dbCmd, "@n_IdAccesorioCama", DbType.Int32, objBE.IdAccesorioCama);
                            this.db.AddInParameter(dbCmd, "@n_IdCama", DbType.Int32, intIdCama);

                            if (this.db.ExecuteNonQuery(dbCmd, dbTr) < 1)
                                return false;
                        }
                    else if (intExiste == 1 && !objBE.Marcado)
                        using (DbCommand dbCmd = this.db.GetSqlStringCommand("delete from Cama_AccesorioCama where IdCama = @n_IdCama and IdAccesorioCama = @n_IdAccesorioCama"))
                        {
                            this.db.AddInParameter(dbCmd, "@n_IdAccesorioCama", DbType.Int32, objBE.IdAccesorioCama);
                            this.db.AddInParameter(dbCmd, "@n_IdCama", DbType.Int32, intIdCama);

                            if (this.db.ExecuteNonQuery(dbCmd, dbTr) < 1)
                                return false;
                        }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool EliminarAccesorios(DbTransaction dbTr, int intIdCama)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("delete from Cama_AccesorioCama where IdCama = @n_IdCama)"))
                {
                    this.db.AddInParameter(dbCmd, "@n_IdCama", DbType.Int32, intIdCama);

                    this.db.ExecuteNonQuery(dbCmd, dbTr);                        
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
