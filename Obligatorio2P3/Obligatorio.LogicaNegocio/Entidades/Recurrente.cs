using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.Entidades
{
    public class Recurrente : Pago
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public override decimal ObtenerMontoTotal()
        {
            var meses = ((FechaHasta.Year - FechaDesde.Year) * 12) + FechaHasta.Month - FechaDesde.Month + 1;
            return Monto * meses;
        }
    }
}
