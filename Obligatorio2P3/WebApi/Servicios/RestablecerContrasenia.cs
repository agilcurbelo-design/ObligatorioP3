namespace Obligatorio.WebApi.Services
{
	public interface IPasswordGeneratorService
	{
		string GenerarPasswordAleatoria();
		bool ValidarPassword(string password);
	}

	public class PasswordGeneratorService : IPasswordGeneratorService
	{
		// Reglas: mínimo 8 caracteres (según tu modelo Usuario)
		// Puedes agregar más reglas si es necesario
		private const int LONGITUD_MINIMA = 8;
		private const int LONGITUD_MAXIMA = 12;
		private const string MAYUSCULAS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string MINUSCULAS = "abcdefghijklmnopqrstuvwxyz";
		private const string NUMEROS = "0123456789";
		private const string ESPECIALES = "!@#$%&*";

		public string GenerarPasswordAleatoria()
		{
			var random = new Random();
			var longitud = random.Next(LONGITUD_MINIMA, LONGITUD_MAXIMA + 1);
			var password = new List<char>();

			// Garantizar al menos un carácter de cada tipo para hacerla más segura
			password.Add(MAYUSCULAS[random.Next(MAYUSCULAS.Length)]);
			password.Add(MINUSCULAS[random.Next(MINUSCULAS.Length)]);
			password.Add(NUMEROS[random.Next(NUMEROS.Length)]);
			password.Add(ESPECIALES[random.Next(ESPECIALES.Length)]);

			// Rellenar el resto con caracteres aleatorios
			var todosCaracteres = MAYUSCULAS + MINUSCULAS + NUMEROS + ESPECIALES;
			for (int i = 4; i < longitud; i++)
			{
				password.Add(todosCaracteres[random.Next(todosCaracteres.Length)]);
			}

			// Mezclar para que no sea predecible
			return new string(password.OrderBy(x => random.Next()).ToArray());
		}

		public bool ValidarPassword(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
				return false;

			if (password.Length < LONGITUD_MINIMA)
				return false;

			return true;
		}
	}
}