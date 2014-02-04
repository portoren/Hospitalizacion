using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BusinessEntities
{
    public partial class BEAltaMedica
    {
        public int IdOrdenInternamientoBitacora { get; set; }
        public string Numero { get; set; }
        public int idDoctor { get; set; }
        public int idCama { get; set; }
        public string Estado { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar un paciente")]
        [Display(Name = "Paciente")]
        public string idPaciente { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar una habitación")]
        [Display(Name = "Tipo de habitacion")]
        public string idHabitacion { get; set; }
        public string NumeroHabitacion { get; set; }

        public string fechaHora { get; set; }

        [Required(ErrorMessage = "Se debe ingresar una descripcion")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "Número de Paciente")]
        public string dni { get; set; }

        //[Display(Name = "Nombre del Paciente")]
        public string NombrePaciente { get; set; }

        //[Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        //[Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        //[Display(Name = "Número de Orden Internamiento")]
        public string NumeroOrdenInternamiento { get; set; }

        public List<BECama> CamasDisponibles { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar el tipo de cama.")]
        [Display(Name = "Tipo de cama")]
        public string TipoCama { get; set; }

        public string Accion { get; set; }
        public string AccionTexto { get; set; }
        public string NombreDoctor { get; set; }
    }
}
