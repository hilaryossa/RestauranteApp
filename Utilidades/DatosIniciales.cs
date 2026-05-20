using RestauranteReservas.Modelo;
using RestauranteReservas.Servicios;

namespace RestauranteReservas.Utilidades
{
    /// <summary>
    /// Carga datos de ejemplo para que la app inicie con contenido demostrable.
    /// </summary>
    public static class DatosIniciales
    {
        public static void Cargar(GestorMesas gestorMesas, GestorClientes gestorClientes,
                                  GestorMenu gestorMenu, GestorReservas gestorReservas)
        {
            // Mesas
            gestorMesas.Agregar(new Mesa(1, 2, "Interior"));
            gestorMesas.Agregar(new Mesa(2, 4, "Interior"));
            gestorMesas.Agregar(new Mesa(3, 4, "Terraza"));
            gestorMesas.Agregar(new Mesa(4, 6, "Terraza"));
            gestorMesas.Agregar(new Mesa(5, 8, "Privado"));
            gestorMesas.Agregar(new Mesa(6, 2, "Interior"));

            // Clientes
            var c1 = new Cliente("Ana García",    "3001234567", "ana@email.com");
            var c2 = new Cliente("Luis Pérez",    "3109876543", "luis@email.com");
            var c3 = new Cliente("María Torres",  "3204567890", "maria@email.com");
            gestorClientes.Agregar(c1);
            gestorClientes.Agregar(c2);
            gestorClientes.Agregar(c3);

            // Menú
            gestorMenu.Agregar(new PlatoEntrada("Ensalada César",      18000, "Lechuga romana, crutones y aderezo", true));
            gestorMenu.Agregar(new PlatoEntrada("Tabla de quesos",     25000, "Selección de quesos artesanales"));
            gestorMenu.Agregar(new PlatoPrincipal("Lomo al trapo",     65000, "Lomo fino al carbón", "Res"));
            gestorMenu.Agregar(new PlatoPrincipal("Pechuga a la plancha", 42000, "Con verduras salteadas", "Pollo"));
            gestorMenu.Agregar(new PlatoPrincipal("Trucha al ajillo",  48000, "Con papas gratinadas", "Pescado"));
            gestorMenu.Agregar(new Postre("Tiramisú",                  22000, "Receta italiana clásica"));
            gestorMenu.Agregar(new Postre("Brownie sin gluten",        20000, "Con helado de vainilla", false));

            // Reservas de ejemplo
            var mesa2 = gestorMesas.BuscarPorNumero(2)!;
            gestorReservas.Crear(c1, mesa2, DateTime.Now.AddHours(2), 3, "Sin mariscos");

            var mesa4 = gestorMesas.BuscarPorNumero(4)!;
            gestorReservas.Crear(c2, mesa4, DateTime.Now.AddDays(1), 5, "Cumpleaños");
        }
    }
}
