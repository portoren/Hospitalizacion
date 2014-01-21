using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BusinessEntities
{

    public partial class BEProcedimientoPregunta
    {

        public int IdProcedimientoPregunta { get; set; }

        public int IdProcedimiento { get; set; }

        public string Tipo { get; set; }

        public string Pregunta { get; set; }

        public string Respuesta { get; set; }

        public string Descripcion { get; set; }

        public string Estado { get; set; }

    }

    public partial class BEProcedimientoPregunta
    {

        public BEProcedimientoPregunta()
        {
        }

    }

}
