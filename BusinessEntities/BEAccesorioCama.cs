using System;
using System.Data;

namespace BusinessEntities
{

    public partial class BEAccesorioCama
    {

        public int IdAccesorioCama { get; set; }
        public string Nombre { get; set; }
        public bool Marcado { get; set; }

    }

    public partial class BEAccesorioCama
    {

        public BEAccesorioCama()
        {
        }

        public BEAccesorioCama(IDataReader reader)
        {
            IdAccesorioCama = Convert.ToInt32(reader["IdAccesorioCama"]);
            Nombre = Convert.ToString(reader["Nombre"]);
            Marcado = false;
        }

    }

}
