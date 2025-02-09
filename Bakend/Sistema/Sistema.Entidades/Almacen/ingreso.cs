﻿using Sistema.Entidades.Usuarios;
using Sistema.Entidades.Ventas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sistema.Entidades.Almacen
{
    public class ingreso
    {
		public int idingreso { get; set; }
		[Required]
		public int idproveedor { get; set; }
		[Required]
		public int idusuario { get; set; }
		[Required]
		public string tipo_comprobante { get; set; }
		public string serie_comprobante { get; set; }
		[Required]
		public string num_comprobante { get; set; }
		[Required]
		public DateTime fecha_hora { get; set; }
		[Required]
		public decimal impuesto { get; set; }
		[Required]
		public decimal total { get; set; }
		[Required]
		public string estado { get; set; }

		public ICollection<DetalleIngreso> detalles { get; set; }
		public Usuario Usuario { get; set; }
		public Persona Persona { get; set; }

	}
}
