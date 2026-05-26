namespace RestauranteReservas.Modelo
{
    public class Cliente : Persona
    {
        private static int _contadorId = 1;

        public int Id { get; private set; }
        public int TotalReservas { get; set; }

        public Cliente(string nombre, string telefono, string email)
            : base(nombre, telefono, email)
        {
            Id            = _contadorId++;
            TotalReservas = 0;
        }

        // Override polimorfismo
        public override string ObtenerInfo()
        {
            return $"[Cliente #{Id}] {base.ObtenerInfo()} | Reservas: {TotalReservas}";
        }
    }
}
