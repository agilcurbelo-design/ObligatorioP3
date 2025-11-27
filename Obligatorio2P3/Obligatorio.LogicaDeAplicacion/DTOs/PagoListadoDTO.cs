using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaDeAplicacion.DTOs
{
    public class PagoListadoDTO
	{
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal SaldoPendiente { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string TipoGasto { get; set; } = string.Empty;
    }

}
