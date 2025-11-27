using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Obligatorio.LogicaNegocio.Entidades
{
    public class Usuario
    {
        // Constructor principal para crear usuarios desde la aplicación
        public Usuario(string nombre, string apellido, string email, string contrasenia, Rol rol)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio", nameof(nombre));

            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido es obligatorio", nameof(apellido));

            if (string.IsNullOrWhiteSpace(contrasenia) || contrasenia.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres", nameof(contrasenia));

            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Contrasenia = contrasenia;
            Rol = rol;

        }

        // Constructor sin parámetros requerido por EF Core
        protected Usuario() { }

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Contrasenia { get; set; } = string.Empty;

        [Required]
        public string Email { get; private set; } = string.Empty;

        [Required]
        public Rol Rol { get; set; }

		public int? EquipoId { get; set; } = 1;
        public virtual Equipo? Equipo { get; set; }

		public ICollection<Pago> Pagos { get; set; } = new List<Pago>();


		public void GenerarEmail(Func<string, bool> existeEmail)
        {
            string nombreNorm = Normalizar(Nombre);
            string apellidoNorm = Normalizar(Apellido);

            string parteNombre = nombreNorm.Length >= 3 ? nombreNorm.Substring(0, 3) : nombreNorm;
            string parteApellido = apellidoNorm.Length >= 3 ? apellidoNorm.Substring(0, 3) : apellidoNorm;

            string baseEmail = (parteNombre + parteApellido).ToLower();
            string emailGenerado = $"{baseEmail}@laempresa.com";

            Random rnd = new Random();
            while (existeEmail(emailGenerado))
            {
                int numero = rnd.Next(1000, 9999);
                emailGenerado = $"{baseEmail}{numero}@laempresa.com";
            }

            Email = emailGenerado;
        }

        private string Normalizar(string texto)
        {
            string normalizado = texto.ToLowerInvariant();
            normalizado = normalizado.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalizado)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            string sinTildes = sb.ToString().Normalize(NormalizationForm.FormC);
            return sinTildes.Replace("ñ", "n");
        }
    }
}