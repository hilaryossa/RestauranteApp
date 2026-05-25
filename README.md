##Sistema de Reservas – Restaurante

Aplicación de escritorio en C# que permite gestionar reservas, clientes, mesas y el menú de un restaurante. Desarrollada como proyecto final del curso **Herramientas de Programación I**.

---

##Integrantes

| Nombre |
|---|
| Hilary David Ossa |
| Sofia Hidalgo Cordoba | 
| Emily Quintero Rivera | 

---

##Descripción del problema

Un restaurante necesita gestionar sus reservas de mesas de forma organizada. Los clientes llaman para reservar una mesa en una fecha y hora específica; el personal debe poder registrar, confirmar, cancelar y completar reservas, así como administrar las mesas disponibles y el menú.

---

##Objetivo del sistema

- Registrar clientes con sus datos de contacto.
- Administrar las mesas del restaurante (número, capacidad, ubicación y estado).
- Crear, listar, confirmar, cancelar y completar reservas.
- Gestionar el menú con entradas, platos principales y postres.
- Validar entradas del usuario y mostrar mensajes de error claros.

---

##Tecnologías utilizadas

- **C#** con **.NET 8.0**
- **Windows Forms (WinForms)**
- **Visual Studio 2022** o **Visual Studio Code**
- **Git** y **GitHub**

---

##Requisitos previos

- .NET SDK 8.0 o superior instalado 
- Windows 10/11 (WinForms requiere Windows)
- Visual Studio 2022 o VS Code con extensión C#

---

##Funcionalidades principales

| Módulo | Funcionalidades |
|---|---|
| **Reservas** | Crear, listar, confirmar, cancelar, completar y eliminar reservas |
| **Clientes** | Registrar, buscar por nombre y eliminar clientes |
| **Mesas** | Agregar mesas, ver estado (disponible/reservada/ocupada), eliminar |
| **Menú** | Agregar entradas, platos principales y postres; eliminar platos |

---

##Estructura del proyecto

```
RestauranteReservas/
├── Modelo/
│   ├── Persona.cs          # Clase base abstracta (herencia)
│   ├── Cliente.cs          # Subclase de Persona
│   ├── Empleado.cs         # Subclase abstracta + Mesero + Anfitrión
│   ├── Mesa.cs             # Entidad Mesa con estados
│   ├── Plato.cs            # Clase abstracta + PlatoEntrada + PlatoPrincipal + Postre
│   └── Reserva.cs          # Entidad Reserva + Pedido
├── Servicios/
│   ├── GestorClientes.cs   # CRUD de clientes
│   ├── GestorMesas.cs      # CRUD de mesas
│   ├── GestorReservas.cs   # CRUD + estados de reservas
│   └── GestorMenu.cs       # CRUD del menú
├── Vista/
│   ├── FormPrincipal.cs    # Ventana principal con dashboard
│   ├── FormClientes.cs     # Gestión de clientes
│   ├── FormMesas.cs        # Gestión de mesas
│   ├── FormReservas.cs     # Gestión de reservas
│   └── FormMenu.cs         # Gestión del menú
├── Utilidades/
│   ├── Validador.cs        # Validaciones y formatos
│   └── DatosIniciales.cs   # Datos de ejemplo al iniciar
├── Diagramas/
│   └── diagrama-clases.png
├── Program.cs
└── RestauranteReservas.csproj
```

---

##Arquitectura

```
[ Vista / FormXxx ]
       ↓ llama a
[ Servicios / GestorXxx ]
       ↓ manipula
[ Modelo / Clases del dominio ]
       ↑ validado por
[ Utilidades / Validador ]
```

- La **Vista** recibe datos del usuario y los pasa a los servicios.
- Los **Servicios** validan, coordinan la lógica de negocio y operan sobre el modelo.
- El **Modelo** representa los objetos del dominio con sus reglas internas.
- Las **Utilidades** proveen funciones reutilizables de validación y formato.

---

##Diagrama UML

![Diagrama de clases](Diagramas/diagrama-clases.png)

###Jerarquías de herencia

```
Persona (abstracta)
├── Cliente
└── Empleado (abstracta)
    ├── Mesero
    └── Anfitrión

Plato (abstracta)
├── PlatoEntrada
├── PlatoPrincipal
└── Postre
```

---

##Capturas de pantalla

```markdown
![Ventana principal](docs/captura-principal.png)
![Gestión de reservas](docs/captura-reservas.png)
![Gestión de clientes](docs/captura-clientes.png)
```

---

##Conceptos de POO aplicados

| Concepto | Dónde se aplica |
|---|---|
| **Encapsulamiento** | Propiedades con validación en `Persona`, `Mesa`, `Plato`, `Reserva` |
| **Herencia** | `Cliente` y `Empleado` extienden `Persona`; `Mesero` y `Anfitrión` extienden `Empleado`; `PlatoEntrada`, `PlatoPrincipal` y `Postre` extienden `Plato` |
| **Polimorfismo** | `ObtenerInfo()` virtual en `Persona`, override en `Cliente`, `Empleado`, `Mesero`, `Anfitrión`; `ObtenerCategoria()` abstracto en `Plato`, implementado en subclases |
| **Abstracción** | `Persona`, `Empleado` y `Plato` son clases abstractas |
| **Colecciones genéricas** | `List<T>` en todos los gestores; `IReadOnlyList<T>` para exponer datos de solo lectura |

---

##Limitaciones conocidas

- Los datos se almacenan solo en memoria; al cerrar la aplicación se pierden.
- No implementa autenticación de usuarios.
- No genera reportes exportables.

---

##Mejoras futuras

- Persistencia en archivos JSON o base de datos SQLite.
- Módulo de pedidos asociados a reservas.
- Exportación de reportes a PDF.
- Autenticación con roles (mesero vs administrador).
- Interfaz con tema oscuro.

---

##Declaración de uso de IA

```
Herramienta utilizada: ...
Propósito: ...
Solicitudes principales: ...
Partes impactadas: ...
Adaptaciones del equipo: ...
Validación realizada: ...
Responsables: ...
Declaración: el equipo comprende el código entregado y puede explicarlo en la sustentación.
```

---

##Licencia y créditos

Proyecto académico desarrollado para el curso Herramientas de Programación I.  
Sin licencia comercial.
