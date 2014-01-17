using System;
using System.Data;

namespace BusinessEntities
{

    public partial class BEHabitacion
    {

        public int IdHabitacion { get; set; }
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public int Disponible { get; set; }

    }

    public partial class BEHabitacion
    {

        public BEHabitacion()
        {            
        }

        public BEHabitacion(IDataReader reader)
        {
            IdHabitacion = Convert.ToInt32(reader["IdHabitacion"]);
            Nombre = Convert.ToString(reader["Numero"]);
        }

    }

    public partial class BEHabitacion
    {

        public string Nombre { get; set; }

    }

    public partial class BEHabitacion
    {
        
        public static readonly int DISPONIBLE_Si = 0;
        public static readonly int DISPONIBLE_No = 1;

    }

}
