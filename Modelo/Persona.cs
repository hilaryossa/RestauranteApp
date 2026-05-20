namespace RestauranteReservas.Modelo
{
    // Clase base abstracta - herencia aplicada
    public abstract class Persona
    {
        private string _nombre;
        private string _telefono;
        private string _email;

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío.");
                _nombre = value.Trim();
            }
        }

        public string Telefono
        {
            get => _telefono;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El teléfono no puede estar vacío.");
                _telefono = value.Trim();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El email no puede estar vacío.");
                _email = value.Trim();
            }
        }

        protected Persona(string nombre, string telefono, string email)
        {
            Nombre   = nombre;
            Telefono = telefono;
            Email    = email;
        }

        // Método virtual - polimorfismo
        public virtual string ObtenerInfo()
        {
            return $"Nombre: {Nombre} | Tel: {Telefono} | Email: {Email}";
        }

        public override string ToString() => Nombre;
    }
}
