using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BusinessEntities
{

    public partial class BEOrdenInternamientoBitacora
    {

        public int IdOrdenInternamientoBitacora { get; set; }

        public int IdOrdenInternamiento { get; set; }
        
        public string EstadoPaciente { get; set; }

        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; }

        public string Estado { get; set; }

    }

    public partial class BEOrdenInternamientoBitacora
    {

        public BEOrdenInternamientoBitacora()
        {
        }

        //public BEOrdenInternamientoBitacora(int intOrdenInternamiento, int intCantidad, string strRecurso, string strRecursoNombre)
        //{
        //    IdOrdenInternamiento = intOrdenInternamiento;
        //    Cantidad = intCantidad;
        //    Recurso = strRecurso;
        //    RecursoNombre = strRecursoNombre;
        //    Indicador = 0;
        //}

        public BEOrdenInternamientoBitacora(IDataReader reader)
        {
            IdOrdenInternamientoBitacora = Convert.ToInt32(reader["IdOrdenInternamientoBitacora"]);
            IdOrdenInternamiento = Convert.ToInt32(reader["IdOrdenInternamiento"]);
            Fecha = Convert.ToDateTime(reader["Fecha"]);
            EstadoPaciente = Convert.ToString(reader["EstadoPaciente"]);
            EstadoPacienteNombre = Convert.ToString(reader["EstadoPacienteNombre"]);
            Estado = Convert.ToString(reader["Estado"]);
            Descripcion = Convert.ToString(reader["Descripcion"]);
        }

    }

    public partial class BEOrdenInternamientoBitacora
    {
                
        public string EstadoPacienteNombre { get; set; }
        
    }

    public partial class BEOrdenInternamientoBitacora
    {

        public static readonly string ESTADO_Activo = "001";
        public static readonly string ESTADO_Anulado = "002";

    }

}
