using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Almacen.Categoria
{
    public class CrearViewModel
    {

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre no debe tener más de 50 caracteres, ni menos de 3")]
        public string nombre { get; set; }
        [StringLength(256)]
        public string descripcion { get; set; }
    }
}
