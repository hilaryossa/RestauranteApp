using System.Text.RegularExpressions;

namespace RestauranteReservas.Utilidades
{
    public static class Validador
    {
        public static bool EsNombreValido(string nombre)
            => !string.IsNullOrWhiteSpace(nombre) && nombre.Trim().Length >= 2;

        public static bool EsTelefonoValido(string telefono)
            => !string.IsNullOrWhiteSpace(telefono) &&
               Regex.IsMatch(telefono.Trim(), @"^[\d\s\+\-\(\)]{7,15}$");

        public static bool EsEmailValido(string email)
            => !string.IsNullOrWhiteSpace(email) &&
               Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public static bool EsFechaValida(DateTime fecha)
            => fecha >= DateTime.Now;

        public static bool EsCapacidadValida(int capacidad)
            => capacidad >= 1 && capacidad <= 20;

        public static bool EsPrecioValido(string texto, out decimal precio)
            => decimal.TryParse(texto, out precio) && precio >= 0;

        public static bool EsEnteroPositivo(string texto, out int valor)
            => int.TryParse(texto, out valor) && valor > 0;
    }

    public static class Formatos
    {
        public static string FormatearMoneda(decimal valor) => $"${valor:0.00}";
        public static string FormatearFecha(DateTime fecha) => fecha.ToString("dd/MM/yyyy HH:mm");
        public static string FormatearFechaSolo(DateTime fecha) => fecha.ToString("dd/MM/yyyy");
    }
}
