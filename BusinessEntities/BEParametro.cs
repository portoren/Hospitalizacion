using System;
using System.Data;

namespace BusinessEntities
{

    public partial class BEParametro
    {

        public string IdParametro { get; set; }
        public string Nombre { get; set; }

    }

    public partial class BEParametro
    {

        public BEParametro()
        {            
        }

        public BEParametro(IDataReader reader)
        {
            IdParametro = Convert.ToString(reader["IdParametro"]);
            Nombre = Convert.ToString(reader["Nombre"]);
        }

    }

    public partial class BEParametro
    {
        
        public static readonly string DOMINIO_Marca = "001";
        public static readonly string DOMINIO_TipoCama = "002";
        public static readonly string DOMINIO_ModoOperacion = "003";
        public static readonly string DOMINIO_TipoColchon = "004";
        public static readonly string DOMINIO_EstadoCama = "005";
        public static readonly string DOMINIO_TipoHabitacion = "006";
        public static readonly string DOMINIO_EstadoOrdenInternamiento = "007";
        public static readonly string DOMINIO_Recurso = "008";
        public static readonly string DOMINIO_EstadoPaciente = "009";
        public static readonly string DOMINIO_Procedimiento = "010";
        public static readonly string DOMINIO_Entrada = "011";
        public static readonly string DOMINIO_PausaQuirurgica = "012";
        public static readonly string DOMINIO_Salida = "013";

    }

}
