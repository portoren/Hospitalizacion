using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using BusinessEntities;

namespace DataAccess
{
    public class DAAltaMedica: DABase
    {
        public bool NuevaAltaMedica(BEAltaMedica objBEA)
        {
            bool isOK = false;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    isOK = InsertarAltaMedica(dbTr, objBEA);

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
        private bool InsertarAltaMedica(DbTransaction dbTr, BEAltaMedica objBEA)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("[pa_AltaMedica_set_insert]"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idOrdenInternamiento", DbType.Int32, objBEA.NumeroOrdenInternamiento);
                    //this.db.AddInParameter(dbCmd, "@n_idDoctor", DbType.Int32, objBEA.idDoctor);
                    //this.db.AddInParameter(dbCmd, "@v_idPaciente", DbType.String, objBEA.idPaciente);
                    //this.db.AddInParameter(dbCmd, "@n_idHabitacion", DbType.Int32, objBEA.idHabitacion);
                    //this.db.AddInParameter(dbCmd, "@n_idCama", DbType.Int32, objBEA.idCama);
                    //this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, objBEA.Estado);
                    this.db.AddInParameter(dbCmd, "@v_descripcion", DbType.String, objBEA.Descripcion);

                    if (this.db.ExecuteNonQuery(dbCmd, dbTr) > 0)
                    {
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
        public List<BEAltaMedica> BuscarPaciente(string nombre)
        {
            List<BEAltaMedica> ListaBusqueda = new List<BEAltaMedica>();
            try
            {
                //if (nombre.Trim().Length > 0)
                //{
                    using (DbCommand dbCmd = this.db.GetStoredProcCommand("[pa_BuscarAltaMedica]"))
                    {
                        string[] NombreCompleto = nombre.Split(' ');
                        this.db.AddInParameter(dbCmd, "@v_nombre", DbType.String, NombreCompleto.Length > 0? NombreCompleto[0] : " ");
                        this.db.AddInParameter(dbCmd, "@v_apellido", DbType.String, NombreCompleto.Length > 1 ? NombreCompleto[1] : " ");
                        this.db.AddInParameter(dbCmd, "@v_apellido_materno", DbType.String, NombreCompleto.Length > 2 ? NombreCompleto[2] : " ");

                        using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                        {
                            while (reader.Read())
                                ListaBusqueda.Add(new BEAltaMedica()
                                {
                                    IdOrdenInternamientoBitacora = int.Parse(reader[0].ToString()),
                                    dni = reader[1].ToString(),
                                    NombrePaciente = reader[2].ToString(),
                                    fechaHora = reader[3].ToString(),
                                    NumeroOrdenInternamiento = reader[4].ToString(),
                                    Accion = reader[5].ToString(),
                                    AccionTexto = reader[6].ToString(),
                                });
                        }
                    }
                //}
            }
            catch (Exception)
            {
                throw;
            }
            return ListaBusqueda;
        }
        public BEAltaMedica BuscarPaciente(int id)
        {
            BEAltaMedica be2 = new BEAltaMedica();
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("[pa_BuscarPaciente]"))
                {
                    this.db.AddInParameter(dbCmd, "@n_OrdenInternamientoBitacora", DbType.Int32, id);
                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                        {
                            be2.IdOrdenInternamientoBitacora = int.Parse(reader[0].ToString());
                            be2.NombrePaciente = reader[1].ToString();
                            be2.ApellidoPaterno = reader[2].ToString();
                            be2.ApellidoMaterno = reader[3].ToString();
                            be2.NumeroOrdenInternamiento = reader[4].ToString();
                            be2.idHabitacion = reader[5].ToString();
                            be2.fechaHora = reader[6].ToString();
                            be2.Descripcion = reader[7].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return be2;
        }
        public List<BEDoctor> ObtenerDoctores()
        {
            List<BEDoctor> ListaDoctores = new List<BEDoctor>();
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("[pa_ObtenerDoctores]"))
                    {
                        using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                        {
                            while (reader.Read())
                                ListaDoctores.Add(new BEDoctor()
                                {
                                    idDoctor = int.Parse(reader[0].ToString()),
                                    NombreDoctor = reader[1].ToString(),
                                });
                        }
                    }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListaDoctores;
        }
        public List<BEPacientes> ObtenerPacientes()
        {
            List<BEPacientes> ListaPacientes = new List<BEPacientes>();
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("[pa_ObtenerPacientes]"))
                {
                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            ListaPacientes.Add(new BEPacientes()
                            {
                                dni = reader[0].ToString(),
                                NombrePaciente = reader[1].ToString(),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListaPacientes;
        }

        public bool ActualizarAltaMedica(BEAltaMedica BEA)
        {
            bool isOK = false;

            try
            {
                DbConnection dbCn = this.db.CreateConnection();
                dbCn.Open();

                DbTransaction dbTr = dbCn.BeginTransaction();

                try
                {
                    isOK = ActualizarAltaMedica_(dbTr, BEA);

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
        private bool ActualizarAltaMedica_(DbTransaction dbTr, BEAltaMedica objBEA)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("[pa_ActualizarAltaMedica]"))
                {
                    this.db.AddInParameter(dbCmd, "@n_OrdenInternamientoBitacora", DbType.Int32, objBEA.IdOrdenInternamientoBitacora);
                    //this.db.AddInParameter(dbCmd, "@v_Nombre", DbType.String, objBEA.NombrePaciente);
                    //this.db.AddInParameter(dbCmd, "@v_ApellidoPaterno", DbType.String, objBEA.ApellidoPaterno);
                    //this.db.AddInParameter(dbCmd, "@v_ApellidoMaterno", DbType.String, objBEA.ApellidoMaterno);
                    //this.db.AddInParameter(dbCmd, "@n_idHabitacion", DbType.String, objBEA.idHabitacion);
                    this.db.AddInParameter(dbCmd, "@v_Descripcion", DbType.String, objBEA.Descripcion);

                    if (this.db.ExecuteNonQuery(dbCmd, dbTr) > 0)
                    {
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

        public BEAltaMedica BuscarPaciente_Registro(int idOrdenInternamiento)
        {
            BEAltaMedica be2 = new BEAltaMedica();
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("[pa_BuscarPaciente_Registro]"))
                {
                    this.db.AddInParameter(dbCmd, "@n_OrdenInternamiento", DbType.Int32, idOrdenInternamiento);
                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                        {
                            be2.NumeroOrdenInternamiento = reader[0].ToString();
                            be2.NombrePaciente = reader[1].ToString();
                            be2.NombreDoctor = reader[2].ToString();
                            be2.idHabitacion = reader[3].ToString();
                            be2.idCama = int.Parse(reader[4].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return be2;
        }
    }
}
