﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Usuarios.Usuario
{
    public class ActualizarViewModel
    {
		[Required]
		public int idusuario { get; set; }
		[Required]
		public int idrol { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "El Nombre no debe tener más de 50 caracteres, ni menos de 3")]
		public string nombre { get; set; }
		public string tipo_documento { get; set; }
		public string num_documento { get; set; }
		public string direccion { get; set; }
		public string telefono { get; set; }
		[Required]
		[EmailAddress]
		public string email { get; set; }
		[Required]
		public string password { get; set; }
        public bool act_password { get; set; }
    }
}
