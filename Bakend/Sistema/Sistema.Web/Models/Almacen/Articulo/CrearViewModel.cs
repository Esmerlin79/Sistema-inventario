using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Almacen.Articulo
{
    public class CrearViewModel
    {
		[Required]
		public int idcategoria { get; set; }
		public string codigo { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre no debe tener más de 50 caracteres, ni menos de 3")]
		public string nombre { get; set; }
		[Required]
		public decimal precio_venta { get; set; }
		[Required]
		public int stock { get; set; }
		public string descripcion { get; set; }
	}
}
