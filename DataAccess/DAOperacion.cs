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

    public class DAOperacion : DABase
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
    h.Numero as Habitacion,
	(select top 1 Estado from Procedimiento where IdOrdenInternamiento = oi.IdOrdenInternamiento and Estado in ('011','012')) as Estado
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
                            lista.Add(new BEOrdenInternamiento(reader, 0));
                    }

                    return lista;
                }
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

                return objBE;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
