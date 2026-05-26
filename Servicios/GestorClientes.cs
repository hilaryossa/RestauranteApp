using RestauranteReservas.Modelo;

namespace RestauranteReservas.Servicios
{
    public class GestorClientes
    {
        private readonly List<Cliente> _clientes = new();

        public IReadOnlyList<Cliente> ObtenerTodos() => _clientes.AsReadOnly();

        public void Agregar(Cliente cliente)
        {
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));
            if (_clientes.Any(c => c.Email.Equals(cliente.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Ya existe un cliente con el email '{cliente.Email}'.");
            _clientes.Add(cliente);
        }

        public Cliente? BuscarPorId(int id)
            => _clientes.FirstOrDefault(c => c.Id == id);

        public List<Cliente> BuscarPorNombre(string nombre)
            => _clientes.Where(c => c.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();

        public void Eliminar(int id)
        {
            var cliente = BuscarPorId(id)
                ?? throw new InvalidOperationException($"No se encontró cliente con ID {id}.");
            _clientes.Remove(cliente);
        }

        public int TotalClientes() => _clientes.Count;
    }
}
