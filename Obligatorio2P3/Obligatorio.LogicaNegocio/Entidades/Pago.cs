using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.Entidades
{
    public abstract class Pago
    {
            public int Id { get; set; }
            public decimal Monto { get; set; }
            public MetodoPago MetodoPago { get; set; }
            public string Descripcion { get; set; } = string.Empty;

            public int TipoGastoId { get; set; }
            public virtual TipoGasto TipoGasto { get; set; } = null!;

		public int UsuarioId { get; set; }
		public Usuario Usuario { get; set; }


		// Fecha genérica para consultas
		public DateTime Fecha { get; set; }

            public abstract decimal ObtenerMontoTotal();
        }
    }
