using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BusinessEntities
{

    public partial class BEOrdenInternamientoRecurso
    {

        public int IdOrdenInternamientoRecurso { get; set; }

        public int IdOrdenInternamiento { get; set; }
        
        public string Recurso { get; set; }

        public int Cantidad { get; set; }
        
        public string Observacion { get; set; }

    }

    public partial class BEOrdenInternamientoRecurso
    {

        public BEOrdenInternamientoRecurso()
        {
        }

        public BEOrdenInternamientoRecurso(int intOrdenInternamiento, int intCantidad, string strRecurso, string strRecursoNombre)
        {
            IdOrdenInternamiento = intOrdenInternamiento;
            Cantidad = intCantidad;
            Recurso = strRecurso;
            RecursoNombre = strRecursoNombre;
            Indicador = 0;
        }

        public BEOrdenInternamientoRecurso(IDataReader reader)
        {
            IdOrdenInternamientoRecurso = Convert.ToInt32(reader["IdOrdenInternamientoRecurso"]);
            IdOrdenInternamiento = Convert.ToInt32(reader["IdOrdenInternamiento"]);
            Recurso = Convert.ToString(reader["Recurso"]);
            RecursoNombre = Convert.ToString(reader["RecursoNombre"]);
            Cantidad = Convert.ToInt32(reader["Cantidad"]);
            Observacion = Convert.ToString(reader["Observacion"]);
            Indicador = 0;
        }

    }

    public partial class BEOrdenInternamientoRecurso
    {
                
        public string RecursoNombre { get; set; }

        public int Indicador { get; set; }
        
    }

    public partial class BEOrdenInternamientoRecurso
    {

    }

}
