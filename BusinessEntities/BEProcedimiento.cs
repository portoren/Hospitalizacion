using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BusinessEntities
{

    public partial class BEProcedimiento
    {

        public int IdProcedimiento { get; set; }

        public int IdOrdenInternamiento { get; set; }

        public string TipoProcedimiento { get; set; }

        public string Estado { get; set; }

        public List<BEProcedimientoPregunta> Preguntas { get; set; }

    }

    public partial class BEProcedimiento
    {

        public BEProcedimiento()
        {
            this.Preguntas = new List<BEProcedimientoPregunta>();
        }
        
    }
    
}
