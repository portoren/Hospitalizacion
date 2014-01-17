using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BusinessEntities
{

    public partial class BEOrdenInternamiento
    {

        public int IdOrdenInternamiento { get; set; }

        [Display(Name = "Orden Internamiento")]
        public string Numero { get; set; }

        public int IdDoctor { get; set; }
        
        public int IdPaciente { get; set; }        
        
        public int? IdHabitacion { get; set; }

        [Display(Name = "Cama")]
        public int? IdCama { get; set; }

        public string Estado { get; set; }

        public List<BEOrdenInternamientoRecurso> Recursos { get; set; }

    }

    public partial class BEOrdenInternamiento
    {

        public BEOrdenInternamiento()
        {
            this.Recursos = new List<BEOrdenInternamientoRecurso>();
        }

        public BEOrdenInternamiento(IDataReader reader, int intTipo)
        {
            this.Recursos = new List<BEOrdenInternamientoRecurso>();

            switch (intTipo)
            {
                case 0:
                    //IdCama = Convert.ToInt32(reader["IdCama"]);
                    //Marca = Convert.ToString(reader["Marca"]);
                    //Modelo = Convert.ToString(reader["Modelo"]);
                    //TipoCama = Convert.ToString(reader["TipoCama"]);
                    //ModoOperacion = Convert.ToString(reader["ModoOperacion"]);
                    //TipoColchon = Convert.ToString(reader["TipoColchon"]);
                    //Estado = Convert.ToString(reader["Estado"]);
                    break;
                case 1:
                    IdOrdenInternamiento = Convert.ToInt32(reader["IdOrdenInternamiento"]);
                    Numero = Convert.ToString(reader["Numero"]);
                    DoctorNombre = Convert.ToString(reader["Doctor"]);
                    PacienteNombre = Convert.ToString(reader["Paciente"]);

                    if (reader["Habitacion"].Equals(DBNull.Value))
                        HabitacionNombre = "";
                    else
                        HabitacionNombre = Convert.ToString(reader["Habitacion"]);

                    break;
                case 2:
                    IdOrdenInternamiento = Convert.ToInt32(reader["IdOrdenInternamiento"]);
                    Numero = Convert.ToString(reader["Numero"]);
                    PacienteNombre = Convert.ToString(reader["Paciente"]);

                    if (!reader["IdCama"].Equals(DBNull.Value))
                        IdCama = Convert.ToInt32(reader["IdCama"]);
                    else
                        IdCama = 0;

                    break;
                case 3:
                    IdOrdenInternamiento = Convert.ToInt32(reader["IdOrdenInternamiento"]);
                    Numero = Convert.ToString(reader["Numero"]);
                    PacienteNombre = Convert.ToString(reader["Paciente"]);
                    HabitacionNombre = Convert.ToString(reader["Habitacion"]);
                    TipoHabitacion = Convert.ToString(reader["TipoHabitacion"]);
                    IdHabitacion = Convert.ToInt32(reader["IdHabitacion"]);
                    CamaNombre = Convert.ToString(reader["Cama"]);

                    break;
                default:
                    break;
            }
        }

    }

    public partial class BEOrdenInternamiento
    {

        [Display(Name = "Paciente")]
        public string PacienteNombre { get; set; }
        
        [Display(Name = "Doctor")]
        public string DoctorNombre { get; set; }
                
        [Display(Name = "Habitacion")]
        public string HabitacionNombre { get; set; }

        [Display(Name = "Cama")]
        public string CamaNombre { get; set; }

        [Display(Name = "Tipo de Habitacion")]
        public string TipoHabitacion { get; set; }

    }

    public partial class BEOrdenInternamiento
    {
        
        public static readonly string ESTADO_NoAsignado = "001";
        public static readonly string ESTADO_Asignado = "002";
        public static readonly string ESTADO_Alta = "003";

    }

}
