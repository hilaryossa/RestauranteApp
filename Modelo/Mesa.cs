namespace RestauranteReservas.Modelo
{
    public enum EstadoMesa { Disponible, Reservada, Ocupada }

    public class Mesa
    {
        private int _capacidad;
        private int _numero;

        public int Numero
        {
            get => _numero;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El número de mesa debe ser mayor a 0.");
                _numero = value;
            }
        }

        public int Capacidad
        {
            get => _capacidad;
            set
            {
                if (value < 1 || value > 20)
                    throw new ArgumentException("La capacidad debe estar entre 1 y 20 personas.");
                _capacidad = value;
            }
        }

        public EstadoMesa Estado { get; set; }
        public string Ubicacion  { get; set; }  // Interior, Terraza, Privado

        public Mesa(int numero, int capacidad, string ubicacion = "Interior")
        {
            Numero    = numero;
            Capacidad = capacidad;
            Ubicacion = ubicacion;
            Estado    = EstadoMesa.Disponible;
        }

        public bool EstaDisponible() => Estado == EstadoMesa.Disponible;

        public override string ToString()
            => $"Mesa #{Numero} – Cap: {Capacidad} – {Ubicacion} – {Estado}";
    }
}
