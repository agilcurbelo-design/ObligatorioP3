using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorio.LogicaNegocio.Excepciones
{
    public class UsuarioException : Exception
    {
        public UsuarioException() : base() { }
        public UsuarioException(string? message) : base(message)
        {
        }
        public UsuarioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
