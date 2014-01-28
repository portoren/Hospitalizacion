using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using BusinessEntities;

namespace DataAccess
{

    public class DAProcedimiento : DABase
    {

        public bool Crear(BEProcedimiento objBE)
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
                        isOK = this.InsertarPreguntas(dbTr, objBE.Preguntas, objBE.IdProcedimiento);

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

        public bool Actualizar(BEProcedimiento objBE)
        {
            bool isOK = false;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    if (objBE.Estado.Equals("012"))
                        objBE.IdProcedimiento = this.ObtenerID(objBE.IdOrdenInternamiento, "011");
                    else
                        objBE.IdProcedimiento = this.ObtenerID(objBE.IdOrdenInternamiento, "012");

                    isOK = this.Actualizar(dbTr, objBE);

                    if (isOK)
                        isOK = this.InsertarPreguntas(dbTr, objBE.Preguntas, objBE.IdProcedimiento);

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

        private int ObtenerID(int intIdOI, string strEstado)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select top 1 IdProcedimiento from Procedimiento where IdOrdenInternamiento = @n_IdOI and Estado = @v_Estado order by IdProcedimiento desc"))
                {
                    this.db.AddInParameter(dbCmd, "@n_IdOI", DbType.Int32, intIdOI);
                    this.db.AddInParameter(dbCmd, "@v_Estado", DbType.String, strEstado);

                    return (int)this.db.ExecuteScalar(dbCmd);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool Insertar(DbTransaction dbTr, BEProcedimiento objBE)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("pa_procedimiento_set_insert"))
                {
                    this.db.AddOutParameter(dbCmd, "@n_id", DbType.Int32, objBE.IdProcedimiento);
                    this.db.AddInParameter(dbCmd, "@n_ordeninternamiento", DbType.Int32, objBE.IdOrdenInternamiento);
                    this.db.AddInParameter(dbCmd, "@v_tipo", DbType.String, objBE.TipoProcedimiento);
                    this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, objBE.Estado);

                    if (this.db.ExecuteNonQuery(dbCmd, dbTr) > 0)
                    {
                        objBE.IdProcedimiento = (int)this.db.GetParameterValue(dbCmd, "@n_id");

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

        private bool Actualizar(DbTransaction dbTr, BEProcedimiento objBE)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update Procedimiento set Estado = @v_estado where IdProcedimiento = @n_id"))
                {
                    this.db.AddInParameter(dbCmd, "@n_id", DbType.String, objBE.IdProcedimiento);
                    this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, objBE.Estado);

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

        private bool InsertarPreguntas(DbTransaction dbTr, List<BEProcedimientoPregunta> lstBE, int intIdProcedimiento)
        {
            try
            {
                foreach (BEProcedimientoPregunta objBE in lstBE)
                {
                    using (DbCommand dbCmd = this.db.GetSqlStringCommand("insert into Procedimiento_Pregunta(IdProcedimiento, Tipo, Pregunta, Respuesta, Descripcion, Estado) values (@n_IdProcedimiento, @v_Tipo, @v_Pregunta, @v_Respuesta, @v_Descripcion, @v_Estado)"))
                    {
                        this.db.AddInParameter(dbCmd, "@n_IdProcedimiento", DbType.Int32, intIdProcedimiento);
                        this.db.AddInParameter(dbCmd, "@v_Tipo", DbType.String, objBE.Tipo);
                        this.db.AddInParameter(dbCmd, "@v_Pregunta", DbType.String, objBE.Pregunta);
                        this.db.AddInParameter(dbCmd, "@v_Respuesta", DbType.String, objBE.Respuesta);
                        this.db.AddInParameter(dbCmd, "@v_Descripcion", DbType.String, objBE.Descripcion);
                        this.db.AddInParameter(dbCmd, "@v_Estado", DbType.String, objBE.Estado);

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

    }

}
