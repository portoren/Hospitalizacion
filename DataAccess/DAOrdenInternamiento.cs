using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using BusinessEntities;

namespace DataAccess
{
    
    public class DAOrdenInternamiento: DABase
    {

        public List<BEOrdenInternamiento> Buscar(string strPaciente)
        {
            try
            {
                List<BEOrdenInternamiento> lista = new List<BEOrdenInternamiento>();

                using (DbCommand dbCmd = this.db.GetSqlStringCommand(@"
select 
oi.IdOrdenInternamiento, 
oi.Numero, 
d.nombre + ' ' + d.apellido_paterno as Doctor, 
p.nombre + ' ' + p.apellido_paterno as Paciente, 
h.Numero as Habitacion 
from OrdenInternamiento oi 
inner join Doctor d on d.IdDoctor = oi.IdDoctor 
inner join Paciente p on p.dni = oi.IdPaciente 
left join Habitacion h on h.IdHabitacion = oi.IdHabitacion 
where oi.Estado in ('001','002') and (p.nombre + ' ' + p.apellido_paterno) like '%' + @v_paciente + '%'"))
                {
                    this.db.AddInParameter(dbCmd, "@v_paciente", DbType.String, strPaciente);

                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            lista.Add(new BEOrdenInternamiento(reader, 1));
                    }

                    return lista;
                }
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
                BEOrdenInternamiento objBE = null;

                using (DbCommand dbCmd = this.db.GetSqlStringCommand(@"
select 
oi.IdOrdenInternamiento, 
oi.IdCama, 
oi.Numero, 
p.nombre + ' ' + p.apellido_paterno as Paciente 
from OrdenInternamiento oi 
inner join Paciente p on p.dni = oi.IdPaciente 
where IdOrdenInternamiento = @n_idordeninternamiento"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idordeninternamiento", DbType.Int32, intIdOrdenInternamiento);

                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                        {
                            objBE = new BEOrdenInternamiento(reader, 2);

                            break;
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

        public BEOrdenInternamiento ObtenerOI(int intIdOrdenInternamiento)
        {
            try
            {
                BEOrdenInternamiento objBE = null;

                using (DbCommand dbCmd = this.db.GetSqlStringCommand(@"
select 
oi.IdOrdenInternamiento, 
oi.Numero, 
oi.IdHabitacion, 
p.nombre + ' ' + p.apellido_paterno as Paciente, 
h.Numero as Habitacion, 
'Cama ' + cast(c.IdCama as varchar(100)) + ' - Modelo: ' + c.Modelo as Cama, 
(select top 1 Nombre from Parametro where IdDominio = '006' and IdParametro = h.Tipo) as TipoHabitacion
from OrdenInternamiento oi 
inner join Paciente p on p.dni = oi.IdPaciente 
inner join Habitacion h on h.IdHabitacion = oi.IdHabitacion
inner join Cama c on c.IdCama = oi.IdCama
where IdOrdenInternamiento = @n_idordeninternamiento"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idordeninternamiento", DbType.Int32, intIdOrdenInternamiento);

                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                        {
                            objBE = new BEOrdenInternamiento(reader, 3);

                            break;
                        }
                    }
                }

                using (DbCommand dbCmd = this.db.GetSqlStringCommand(@"
select oir.IdOrdenInternamientoRecurso, oir.IdOrdenInternamiento, oir.Recurso, (select top 1 Nombre from Parametro where IdDominio = '008' and IdParametro = oir.Recurso) as RecursoNombre, 
oir.Cantidad, oir.Observacion from OrdenInternamiento_Recurso oir where oir.IdOrdenInternamiento = @n_idordeninternamiento"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idordeninternamiento", DbType.Int32, intIdOrdenInternamiento);

                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            objBE.Recursos.Add(new BEOrdenInternamientoRecurso(reader));
                        
                    }
                }

                return objBE;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public bool Asignar(BEOrdenInternamiento objBE)
        {
            bool isOK = false;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    isOK = this.Asignar(dbTr, objBE);

                    if (isOK)                                            
                        isOK = this.InsertarRecursos(dbTr, objBE.Recursos);

                    if (isOK)
                        isOK = this.HabitacionOcupada(dbTr, objBE.IdHabitacion.Value);

                    if (isOK)
                        isOK = this.CamaOcupada(dbTr, objBE.IdCama.Value);

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

        private bool Asignar(DbTransaction dbTr, BEOrdenInternamiento objBE)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update OrdenInternamiento set IdHabitacion = @n_IdHabitacion, IdCama = @n_IdCama, Estado = @v_Estado where IdOrdenInternamiento = @n_IdOrdenInternamiento"))
                {
                    this.db.AddInParameter(dbCmd, "@n_IdHabitacion", DbType.String, objBE.IdHabitacion);
                    this.db.AddInParameter(dbCmd, "@n_IdCama", DbType.String, objBE.IdCama.Value);
                    this.db.AddInParameter(dbCmd, "@v_Estado", DbType.String, objBE.Estado);
                    this.db.AddInParameter(dbCmd, "@n_IdOrdenInternamiento", DbType.Int32, objBE.IdOrdenInternamiento);

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

        private bool InsertarRecursos(DbTransaction dbTr, List<BEOrdenInternamientoRecurso> lstBE)
        {
            try
            {
                foreach (BEOrdenInternamientoRecurso objBE in lstBE)
                {
                    using (DbCommand dbCmd = this.db.GetSqlStringCommand("insert into OrdenInternamiento_Recurso (IdOrdenInternamiento, Recurso, Cantidad, Observacion) values (@n_IdOrdenInternamiento, @v_Recurso, @n_Cantidad, '')"))
                    {
                        this.db.AddInParameter(dbCmd, "@n_IdOrdenInternamiento", DbType.Int32, objBE.IdOrdenInternamiento);
                        this.db.AddInParameter(dbCmd, "@v_Recurso", DbType.String, objBE.Recurso);
                        this.db.AddInParameter(dbCmd, "@n_Cantidad", DbType.Int32, objBE.Cantidad);

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

        private bool HabitacionOcupada(DbTransaction dbTr, int intHabitacion)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update Habitacion set Disponible = @n_disponible where IdHabitacion = @n_IdHabitacion"))
                {
                    this.db.AddInParameter(dbCmd, "@n_disponible", DbType.Int32, BEHabitacion.DISPONIBLE_No);
                    this.db.AddInParameter(dbCmd, "@n_IdHabitacion", DbType.Int32, intHabitacion);

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

        private bool CamaOcupada(DbTransaction dbTr, int intCama)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update Cama set Estado = @v_estado where IdCama = @n_IdCama"))
                {
                    this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, BECama.ESTADO_EnUso);
                    this.db.AddInParameter(dbCmd, "@n_IdCama", DbType.Int32, intCama);

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

        public bool Editar(BEOrdenInternamiento objBE)
        {
            bool isOK = true;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    foreach (BEOrdenInternamientoRecurso objBEr in objBE.Recursos)
                    {
                        if (objBEr.IdOrdenInternamientoRecurso == 0)
                            using (DbCommand dbCmd = this.db.GetSqlStringCommand("insert into OrdenInternamiento_Recurso (IdOrdenInternamiento, Recurso, Cantidad, Observacion) values (@n_IdOrdenInternamiento, @v_Recurso, @n_Cantidad, '')"))
                            {
                                this.db.AddInParameter(dbCmd, "@n_IdOrdenInternamiento", DbType.Int32, objBE.IdOrdenInternamiento);
                                this.db.AddInParameter(dbCmd, "@v_Recurso", DbType.String, objBEr.Recurso);
                                this.db.AddInParameter(dbCmd, "@n_Cantidad", DbType.Int32, objBEr.Cantidad);

                                if (this.db.ExecuteNonQuery(dbCmd, dbTr) < 1)
                                    isOK = false;
                            }
                        else if (objBEr.Indicador == 1)
                            using (DbCommand dbCmd = this.db.GetSqlStringCommand("delete from OrdenInternamiento_Recurso where IdOrdenInternamientoRecurso = @n_IdOrdenInternamientoRecurso"))
                            {
                                this.db.AddInParameter(dbCmd, "@n_IdOrdenInternamientoRecurso", DbType.Int32, objBEr.IdOrdenInternamientoRecurso);

                                if (this.db.ExecuteNonQuery(dbCmd, dbTr) < 1)
                                    isOK = false;
                            }

                        if (!isOK)
                            break;
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

        public bool Alta(BEOrdenInternamiento objBE)
        {
            bool isOK = false;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    isOK = this.Alta(dbTr, objBE);

                    if (isOK)
                        isOK = this.ActualizarRecursos(dbTr, objBE.Recursos);

                    if (isOK)
                        isOK = this.HabitacionLibre(dbTr, objBE.IdHabitacion.Value);

                    if (isOK)
                        isOK = this.CamaLiberar(dbTr, objBE.IdOrdenInternamiento);

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

        private bool Alta(DbTransaction dbTr, BEOrdenInternamiento objBE)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update OrdenInternamiento set Estado = @v_Estado where IdOrdenInternamiento = @n_IdOrdenInternamiento"))
                {
                    this.db.AddInParameter(dbCmd, "@v_Estado", DbType.String, objBE.Estado);
                    this.db.AddInParameter(dbCmd, "@n_IdOrdenInternamiento", DbType.Int32, objBE.IdOrdenInternamiento);

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

        private bool ActualizarRecursos(DbTransaction dbTr, List<BEOrdenInternamientoRecurso> lstBE)
        {
            try
            {
                foreach (BEOrdenInternamientoRecurso objBE in lstBE)
                {
                    using (DbCommand dbCmd = this.db.GetSqlStringCommand("update OrdenInternamiento_Recurso set Observacion = @v_observacion where IdOrdenInternamientoRecurso = @n_IdOrdenInternamientoRecurso"))
                    {
                        this.db.AddInParameter(dbCmd, "@v_observacion", DbType.String, objBE.Observacion);
                        this.db.AddInParameter(dbCmd, "@n_IdOrdenInternamientoRecurso", DbType.Int32, objBE.IdOrdenInternamientoRecurso);

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

        private bool HabitacionLibre(DbTransaction dbTr, int intHabitacion)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update Habitacion set Disponible = @n_disponible where IdHabitacion = @n_IdHabitacion"))
                {
                    this.db.AddInParameter(dbCmd, "@n_disponible", DbType.Int32, BEHabitacion.DISPONIBLE_Si);
                    this.db.AddInParameter(dbCmd, "@n_IdHabitacion", DbType.Int32, intHabitacion);

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

        private bool CamaLiberar(DbTransaction dbTr, int intOI)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update Cama set Estado = @v_estado where IdCama in (select IdCama from OrdenInternamiento where IdOrdenInternamiento = @n_IdOI)"))
                {
                    this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, BECama.ESTADO_Abierta);
                    this.db.AddInParameter(dbCmd, "@n_IdOI", DbType.Int32, intOI);

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

        public int ValidarEstado(int intId, string strEstado)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("select count(1) from OrdenInternamiento where IdOrdenInternamiento = @n_Id and Estado = @v_Estado"))
                {
                    this.db.AddInParameter(dbCmd, "@n_Id", DbType.Int32, intId);
                    this.db.AddInParameter(dbCmd, "@v_Estado", DbType.String, strEstado);

                    return (int)this.db.ExecuteScalar(dbCmd);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
