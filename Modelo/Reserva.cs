namespace RestauranteReservas.Modelo
{
    public enum EstadoReserva { Pendiente, Confirmada, Cancelada, Completada }

    public class Reserva
    {
        private static int _contadorId = 1;

        private int _numerPersonas;

        public int Id               { get; private set; }
        public Cliente Cliente      { get; set; }
        public Mesa Mesa            { get; set; }
        public DateTime FechaHora   { get; set; }
        public EstadoReserva Estado { get; set; }
        public string Observaciones { get; set; }

        public int NumeroPersonas
        {
            get => _numerPersonas;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Debe haber al menos 1 persona.");
                _numerPersonas = value;
            }
        }

        public Reserva(Cliente cliente, Mesa mesa, DateTime fechaHora, int numeroPersonas, string observaciones = "")
        {
            Id             = _contadorId++;
            Cliente        = cliente;
            Mesa           = mesa;
            FechaHora      = fechaHora;
            NumeroPersonas = numeroPersonas;
            Observaciones  = observaciones;
            Estado         = EstadoReserva.Pendiente;
        }

        public override string ToString()
            => $"Reserva #{Id} | {Cliente.Nombre} | Mesa {Mesa.Numero} | {FechaHora:dd/MM/yyyy HH:mm} | {NumeroPersonas} personas | {Estado}";
    }

    public class Pedido
    {
        private static int _contadorId = 1;

        public int Id             { get; private set; }
        public Reserva Reserva    { get; set; }
        public List<Plato> Platos { get; private set; }
        public DateTime Hora      { get; private set; }

        public decimal Total => Platos.Sum(p => p.Precio);

        public Pedido(Reserva reserva)
        {
            Id      = _contadorId++;
            Reserva = reserva;
            Platos  = new List<Plato>();
            Hora    = DateTime.Now;
        }

        public void AgregarPlato(Plato plato) => Platos.Add(plato);

        public override string ToString()
            => $"Pedido #{Id} | Reserva #{Reserva.Id} | {Platos.Count} plato(s) | Total: ${Total:0.00}";
    }
}
