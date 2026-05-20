using RestauranteReservas.Modelo;

namespace RestauranteReservas.Servicios
{
    public class GestorMesas
    {
        private readonly List<Mesa> _mesas = new();

        public IReadOnlyList<Mesa> ObtenerTodas() => _mesas.AsReadOnly();

        public List<Mesa> ObtenerDisponibles()
            => _mesas.Where(m => m.EstaDisponible()).ToList();

        public List<Mesa> ObtenerDisponiblesParaCapacidad(int personas)
            => _mesas.Where(m => m.EstaDisponible() && m.Capacidad >= personas).ToList();

        public void Agregar(Mesa mesa)
        {
            if (_mesas.Any(m => m.Numero == mesa.Numero))
                throw new InvalidOperationException($"Ya existe la mesa #{mesa.Numero}.");
            _mesas.Add(mesa);
        }

        public Mesa? BuscarPorNumero(int numero)
            => _mesas.FirstOrDefault(m => m.Numero == numero);

        public void Eliminar(int numero)
        {
            var mesa = BuscarPorNumero(numero)
                ?? throw new InvalidOperationException($"No existe la mesa #{numero}.");
            if (mesa.Estado != EstadoMesa.Disponible)
                throw new InvalidOperationException("No se puede eliminar una mesa que está reservada u ocupada.");
            _mesas.Remove(mesa);
        }

        public void CambiarEstado(int numero, EstadoMesa nuevoEstado)
        {
            var mesa = BuscarPorNumero(numero)
                ?? throw new InvalidOperationException($"No existe la mesa #{numero}.");
            mesa.Estado = nuevoEstado;
        }

        public int TotalMesas() => _mesas.Count;
        public int MesasDisponibles() => _mesas.Count(m => m.EstaDisponible());
    }
}
