using RestauranteReservas.Modelo;

namespace RestauranteReservas.Servicios
{
    public class GestorReservas
    {
        private readonly List<Reserva> _reservas = new();

        public IReadOnlyList<Reserva> ObtenerTodas() => _reservas.AsReadOnly();

        public Reserva Crear(Cliente cliente, Mesa mesa, DateTime fechaHora, int numeroPersonas, string observaciones = "")
        {
            if (!mesa.EstaDisponible())
                throw new InvalidOperationException($"La mesa #{mesa.Numero} no está disponible.");
            if (mesa.Capacidad < numeroPersonas)
                throw new InvalidOperationException($"La mesa #{mesa.Numero} solo tiene capacidad para {mesa.Capacidad} personas.");
            if (fechaHora < DateTime.Now)
                throw new InvalidOperationException("La fecha y hora de la reserva no puede ser en el pasado.");

            var reserva = new Reserva(cliente, mesa, fechaHora, numeroPersonas, observaciones);
            mesa.Estado = EstadoMesa.Reservada;
            cliente.TotalReservas++;
            _reservas.Add(reserva);
            return reserva;
        }

        public Reserva? BuscarPorId(int id)
            => _reservas.FirstOrDefault(r => r.Id == id);

        public List<Reserva> BuscarPorCliente(string nombreCliente)
            => _reservas.Where(r => r.Cliente.Nombre.Contains(nombreCliente, StringComparison.OrdinalIgnoreCase)).ToList();

        public List<Reserva> BuscarPorFecha(DateTime fecha)
            => _reservas.Where(r => r.FechaHora.Date == fecha.Date).ToList();

        public void Cancelar(int id)
        {
            var reserva = BuscarPorId(id)
                ?? throw new InvalidOperationException($"No se encontró la reserva #{id}.");
            if (reserva.Estado == EstadoReserva.Cancelada)
                throw new InvalidOperationException("Esta reserva ya fue cancelada.");
            if (reserva.Estado == EstadoReserva.Completada)
                throw new InvalidOperationException("No se puede cancelar una reserva completada.");

            reserva.Estado      = EstadoReserva.Cancelada;
            reserva.Mesa.Estado = EstadoMesa.Disponible;
        }

        public void Confirmar(int id)
        {
            var reserva = BuscarPorId(id)
                ?? throw new InvalidOperationException($"No se encontró la reserva #{id}.");
            if (reserva.Estado != EstadoReserva.Pendiente)
                throw new InvalidOperationException("Solo se pueden confirmar reservas en estado Pendiente.");
            reserva.Estado = EstadoReserva.Confirmada;
        }

        public void Completar(int id)
        {
            var reserva = BuscarPorId(id)
                ?? throw new InvalidOperationException($"No se encontró la reserva #{id}.");
            reserva.Estado      = EstadoReserva.Completada;
            reserva.Mesa.Estado = EstadoMesa.Disponible;
        }

        public void Eliminar(int id)
        {
            var reserva = BuscarPorId(id)
                ?? throw new InvalidOperationException($"No se encontró la reserva #{id}.");
            if (reserva.Estado == EstadoReserva.Confirmada)
                throw new InvalidOperationException("No se puede eliminar una reserva confirmada. Cancélela primero.");
            if (reserva.Estado != EstadoReserva.Cancelada)
                reserva.Mesa.Estado = EstadoMesa.Disponible;
            _reservas.Remove(reserva);
        }

        public int TotalReservas() => _reservas.Count;
        public int ReservasActivas() => _reservas.Count(r => r.Estado == EstadoReserva.Pendiente || r.Estado == EstadoReserva.Confirmada);
    }
}
