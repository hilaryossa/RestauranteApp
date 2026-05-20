namespace RestauranteReservas.Modelo
{
    // Subclase de Persona - segunda rama de herencia
    public abstract class Empleado : Persona
    {
        private static int _contadorId = 1;

        public int Id       { get; private set; }
        public string Cargo { get; protected set; }

        protected Empleado(string nombre, string telefono, string email, string cargo)
            : base(nombre, telefono, email)
        {
            Id    = _contadorId++;
            Cargo = cargo;
        }

        public override string ObtenerInfo()
        {
            return $"[Empleado #{Id}] Cargo: {Cargo} | {base.ObtenerInfo()}";
        }

        // Método abstracto - polimorfismo obligatorio en subclases
        public abstract string ObtenerResponsabilidad();
    }

    // ── Subclase concreta 1 ──────────────────────────────────────────────────
    public class Mesero : Empleado
    {
        public int MesasAsignadas { get; set; }

        public Mesero(string nombre, string telefono, string email)
            : base(nombre, telefono, email, "Mesero")
        {
            MesasAsignadas = 0;
        }

        public override string ObtenerResponsabilidad()
            => $"Atender mesas asignadas ({MesasAsignadas} mesa(s) actualmente).";

        public override string ObtenerInfo()
            => base.ObtenerInfo() + $" | Mesas asignadas: {MesasAsignadas}";
    }

    // ── Subclase concreta 2 ──────────────────────────────────────────────────
    public class Anfitrion : Empleado
    {
        public int ReservasGestionadas { get; set; }

        public Anfitrion(string nombre, string telefono, string email)
            : base(nombre, telefono, email, "Anfitrión")
        {
            ReservasGestionadas = 0;
        }

        public override string ObtenerResponsabilidad()
            => $"Gestionar reservas y recibir clientes ({ReservasGestionadas} reservas gestionadas).";

        public override string ObtenerInfo()
            => base.ObtenerInfo() + $" | Reservas gestionadas: {ReservasGestionadas}";
    }
}
