using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

using BusinessEntities;

namespace DataAccess
{

    public class DABitacora : DABase
    {

        public List<BEOrdenInternamiento> Buscar(string strApellido, string strNombre)
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
inner join Habitacion h  on h.IdHabitacion = oi.IdHabitacion  
where oi.Estado in ('002') and 
p.apellido_paterno like '%' + @v_apellido + '%' and
p.nombre like '%' + @v_nombre + '%'"))
                {
                    this.db.AddInParameter(dbCmd, "@v_apellido", DbType.String, strApellido);
                    this.db.AddInParameter(dbCmd, "@v_nombre", DbType.String, strNombre);

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
        
        public BEOrdenInternamiento ObtenerBitacora(int intIdOrdenInternamiento)
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
    d.nombre + ' ' + d.apellido_paterno as Doctor
from OrdenInternamiento oi 
inner join Paciente p on p.dni = oi.IdPaciente 
inner join Habitacion h on h.IdHabitacion = oi.IdHabitacion
inner join Doctor d on d.IdDoctor = oi.IdDoctor
where IdOrdenInternamiento = @n_idordeninternamiento"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idordeninternamiento", DbType.Int32, intIdOrdenInternamiento);

                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                        {
                            objBE = new BEOrdenInternamiento(reader, 4);

                            break;
                        }
                    }
                }

                using (DbCommand dbCmd = this.db.GetSqlStringCommand(@"
select 
    oib.IdOrdenInternamientoBitacora, 
    oib.IdOrdenInternamiento, 
    oib.Fecha,
    oib.EstadoPaciente, 
    (select top 1 Nombre from Parametro where IdDominio = '009' and IdParametro = oib.EstadoPaciente) as EstadoPacienteNombre, 
    oib.Descripcion,
    oib.Estado
from OrdenInternamiento_Bitacora oib 
where oib.IdOrdenInternamiento = @n_idordeninternamiento
    and oib.Estado = '001'"))
                {
                    this.db.AddInParameter(dbCmd, "@n_idordeninternamiento", DbType.Int32, intIdOrdenInternamiento);

                    using (IDataReader reader = this.db.ExecuteReader(dbCmd))
                    {
                        while (reader.Read())
                            objBE.Bitacora.Add(new BEOrdenInternamientoBitacora(reader));
                    }
                }

                return objBE;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Crear(BEOrdenInternamientoBitacora objBE)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetStoredProcCommand("pa_bitacora_set_insert"))
                {
                    this.db.AddOutParameter(dbCmd, "@n_id", DbType.Int32, objBE.IdOrdenInternamientoBitacora);
                    this.db.AddInParameter(dbCmd, "@n_ordeninternamiento", DbType.Int32, objBE.IdOrdenInternamiento);
                    this.db.AddInParameter(dbCmd, "@f_fecha", DbType.DateTime, objBE.Fecha);
                    this.db.AddInParameter(dbCmd, "@v_estadopaciente", DbType.String, objBE.EstadoPaciente);
                    this.db.AddInParameter(dbCmd, "@v_descripcion", DbType.String, objBE.Descripcion);
                    this.db.AddInParameter(dbCmd, "@v_estado", DbType.String, objBE.Estado);

                    if (this.db.ExecuteNonQuery(dbCmd) > 0)
                    {
                        objBE.IdOrdenInternamientoBitacora = (int)this.db.GetParameterValue(dbCmd, "@n_id");

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

        public bool Eliminar(int intID)
        {
            try
            {
                using (DbCommand dbCmd = this.db.GetSqlStringCommand("update OrdenInternamiento_Bitacora set Estado = '002' where IdOrdenInternamientoBitacora = @n_id"))
                {
                    this.db.AddInParameter(dbCmd, "@n_id", DbType.Int32, intID);

                    if (this.db.ExecuteNonQuery(dbCmd) > 0)
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
