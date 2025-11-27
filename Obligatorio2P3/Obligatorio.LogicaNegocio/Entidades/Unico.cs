using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.Entidades
{
    public class Unico : Pago
    {
        public string NumeroRecibo { get; set; } = string.Empty;

        public DateTime FechaPago
        {
            get => Fecha;
            set => Fecha = value;
        }

        public override decimal ObtenerMontoTotal() => Monto;
    }
}
