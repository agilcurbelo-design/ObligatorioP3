using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DTOs
{
    public class CrearPagoDTO
    {
        public int UsuarioId { get; set; }                 // se puede obtener del usuario autenticado
        public int TipoGastoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Tipo de pago
        public bool EsRecurrente { get; set; }

        // Campos sólo para recurrente
        public int? PeriodoMeses { get; set; }
        public string Descripcion { get; set; }

    }
}