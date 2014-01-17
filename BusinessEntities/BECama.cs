using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BusinessEntities
{

    public partial class BECama
    {
        
        public int IdCama { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar la marca.")]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Se debe ingresar el modelo.")]
        [Display(Name = "Modelo")]
        [StringLength(30, ErrorMessage = "La longitud del texto para el modelo no puede ser mayor a 30 caracteres.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar el tipo de cama.")]
        [Display(Name = "Tipo de cama")]
        public string TipoCama { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar el modo de operación.")]
        [Display(Name = "Modo de operación")]
        public string ModoOperacion { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar el tipo de colchón.")]
        [Display(Name = "Tipo de colchón")]
        public string TipoColchon { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar por lo menos un accesorio.")] 
        [Display(Name = "Accesorios")]
        public List<BEAccesorioCama> Accesorios { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

    }

    public partial class BECama
    {

        public BECama()
        {
        }

        public BECama(IDataReader reader, int intTipo)
        {
            switch (intTipo)
            {
                case 0:
                    IdCama = Convert.ToInt32(reader["IdCama"]);
                    Marca = Convert.ToString(reader["Marca"]);
                    Modelo = Convert.ToString(reader["Modelo"]);
                    TipoCama = Convert.ToString(reader["TipoCama"]);
                    ModoOperacion = Convert.ToString(reader["ModoOperacion"]);
                    TipoColchon = Convert.ToString(reader["TipoColchon"]);
                    Estado = Convert.ToString(reader["Estado"]);
                    break;
                case 1:
                    IdCama = Convert.ToInt32(reader["IdCama"]);
                    MarcaNombre = Convert.ToString(reader["Marca"]);
                    TipoCamaNombre = Convert.ToString(reader["TipoCama"]);
                    ModoOperacionNombre = Convert.ToString(reader["ModoOperacion"]);
                    TipoColchonNombre = Convert.ToString(reader["TipoColchon"]);
                    ListaAccesorios = Convert.ToString(reader["Accesorios"]);
                    EstadoNombre = Convert.ToString(reader["Estado"]);
                    break;
                case 2:
                    IdCama = Convert.ToInt32(reader["IdCama"]);
                    Nombre = Convert.ToString(reader["Marca"]);
                    break;
                default:
                    break;
            }
        }
    }

    public partial class BECama
    {
        
        [Display(Name = "Marca")]
        public string MarcaNombre { get; set; }

        [Display(Name = "Tipo")]
        public string TipoCamaNombre { get; set; }

        [Display(Name = "Operación")]
        public string ModoOperacionNombre { get; set; }

        [Display(Name = "Colchón")]
        public string TipoColchonNombre { get; set; }

        [Display(Name = "Accesorios")]
        public string ListaAccesorios { get; set; }

        [Display(Name = "Estado")]
        public string EstadoNombre { get; set; }

        public string Nombre { get; set; }

    }

    public partial class BECama
    {
        
        public static readonly string ESTADO_Abierta = "001";
        public static readonly string ESTADO_Cerrada = "002";
        public static readonly string ESTADO_EnUso = "003";
        public static readonly string ESTADO_Anulada = "004";

    }

}
