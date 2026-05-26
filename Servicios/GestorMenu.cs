using RestauranteReservas.Modelo;

namespace RestauranteReservas.Servicios
{
    public class GestorMenu
    {
        private readonly List<Plato> _platos = new();

        public IReadOnlyList<Plato> ObtenerTodos() => _platos.AsReadOnly();

        public List<Plato> ObtenerPorCategoria<T>() where T : Plato
            => _platos.OfType<T>().Cast<Plato>().ToList();

        public void Agregar(Plato plato)
        {
            if (_platos.Any(p => p.Nombre.Equals(plato.Nombre, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Ya existe un plato con el nombre '{plato.Nombre}'.");
            _platos.Add(plato);
        }

        public Plato? BuscarPorNombre(string nombre)
            => _platos.FirstOrDefault(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));

        public void Eliminar(string nombre)
        {
            var plato = _platos.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException($"No existe el plato '{nombre}'.");
            _platos.Remove(plato);
        }

        public int TotalPlatos() => _platos.Count;
    }
}
