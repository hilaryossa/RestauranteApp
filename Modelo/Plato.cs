namespace RestauranteReservas.Modelo
{
    // Clase base para el menú
    public abstract class Plato
    {
        private string _nombre;
        private decimal _precio;

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del plato no puede estar vacío.");
                _nombre = value.Trim();
            }
        }

        public decimal Precio
        {
            get => _precio;
            set
            {
                if (value < 0)
                    throw new ArgumentException("El precio no puede ser negativo.");
                _precio = value;
            }
        }

        public string Descripcion { get; set; }

        protected Plato(string nombre, decimal precio, string descripcion)
        {
            Nombre      = nombre;
            Precio      = precio;
            Descripcion = descripcion;
        }

        public abstract string ObtenerCategoria();

        public override string ToString()
            => $"{Nombre} (${Precio:0.00}) – {ObtenerCategoria()}";
    }

    public class PlatoEntrada : Plato
    {
        public bool EsVegetariano { get; set; }

        public PlatoEntrada(string nombre, decimal precio, string descripcion, bool esVegetariano = false)
            : base(nombre, precio, descripcion)
        {
            EsVegetariano = esVegetariano;
        }

        public override string ObtenerCategoria() => EsVegetariano ? "Entrada Vegetariana" : "Entrada";
    }

    public class PlatoPrincipal : Plato
    {
        public string TipoProteina { get; set; }

        public PlatoPrincipal(string nombre, decimal precio, string descripcion, string tipoProteina)
            : base(nombre, precio, descripcion)
        {
            TipoProteina = tipoProteina;
        }

        public override string ObtenerCategoria() => $"Plato Principal – {TipoProteina}";
    }

    public class Postre : Plato
    {
        public bool ContieneGluten { get; set; }

        public Postre(string nombre, decimal precio, string descripcion, bool contieneGluten = true)
            : base(nombre, precio, descripcion)
        {
            ContieneGluten = contieneGluten;
        }

        public override string ObtenerCategoria() => ContieneGluten ? "Postre" : "Postre Sin Gluten";
    }
}
