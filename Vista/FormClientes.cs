using RestauranteReservas.Modelo;
using RestauranteReservas.Servicios;
using RestauranteReservas.Utilidades;

namespace RestauranteReservas.Vista
{
    public class FormClientes : Form
    {
        private readonly GestorClientes _gestor;

        private TextBox txtNombre   = null!;
        private TextBox txtTelefono = null!;
        private TextBox txtEmail    = null!;
        private TextBox txtBuscar   = null!;
        private ListView lvClientes = null!;
        private Label lblMensaje    = null!;

        public FormClientes(GestorClientes gestor)
        {
            _gestor = gestor;
            InicializarComponentes();
            CargarLista();
        }

        private void InicializarComponentes()
        {
            this.Text            = "👥  Gestión de Clientes";
            this.Size            = new Size(820, 560);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Color.FromArgb(240, 248, 255);
            this.Font            = new Font("Segoe UI", 10);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            // Header
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(33, 150, 243) };
            pnlHeader.Controls.Add(new Label
            {
                Text = "👥  Clientes del Restaurante", ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(15, 12)
            });
            this.Controls.Add(pnlHeader);

            // Panel formulario
            var pnlForm = new GroupBox
            {
                Text     = "Registrar nuevo cliente",
                Location = new Point(15, 60),
                Size     = new Size(380, 200),
                Font     = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            int y = 25;
            foreach (var (lbl, ref_) in new (string, Action<TextBox>)[]
            {
                ("Nombre:", tb => txtNombre = tb),
                ("Teléfono:", tb => txtTelefono = tb),
                ("Email:", tb => txtEmail = tb)
            })
            {
                pnlForm.Controls.Add(new Label { Text = lbl, Location = new Point(10, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9) });
                var tb = new TextBox { Location = new Point(100, y), Size = new Size(260, 26) };
                ref_(tb);
                pnlForm.Controls.Add(tb);
                y += 40;
            }

            var btnAgregar = new Button
            {
                Text = "➕  Registrar Cliente", Location = new Point(10, y + 5),
                Size = new Size(360, 35), BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregar_Click;
            pnlForm.Controls.Add(btnAgregar);
            this.Controls.Add(pnlForm);

            // Buscar
            var pnlBuscar = new Panel { Location = new Point(15, 270), Size = new Size(380, 40) };
            pnlBuscar.Controls.Add(new Label { Text = "Buscar:", Location = new Point(0, 8), AutoSize = true });
            txtBuscar = new TextBox { Location = new Point(60, 5), Size = new Size(220, 26) };
            txtBuscar.TextChanged += (s, e) => CargarLista(txtBuscar.Text);
            var btnBuscar = new Button
            {
                Text = "🔍", Location = new Point(288, 4), Size = new Size(40, 28),
                BackColor = Color.FromArgb(33, 150, 243), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnBuscar.FlatAppearance.BorderSize = 0;
            pnlBuscar.Controls.Add(txtBuscar);
            pnlBuscar.Controls.Add(btnBuscar);
            this.Controls.Add(pnlBuscar);

            // Botones acción
            var btnEliminar = new Button
            {
                Text = "🗑️  Eliminar seleccionado", Location = new Point(15, 318),
                Size = new Size(220, 32), BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            // Mensaje
            lblMensaje = new Label
            {
                Location = new Point(15, 358), Size = new Size(380, 24),
                Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Green
            };
            this.Controls.Add(lblMensaje);

            // ListView
            lvClientes = new ListView
            {
                Location      = new Point(410, 60),
                Size          = new Size(375, 450),
                View          = View.Details,
                FullRowSelect = true,
                GridLines     = true,
                BackColor     = Color.White
            };
            lvClientes.Columns.Add("ID", 40);
            lvClientes.Columns.Add("Nombre", 130);
            lvClientes.Columns.Add("Teléfono", 100);
            lvClientes.Columns.Add("Reservas", 70);
            this.Controls.Add(lvClientes);
        }

        private void CargarLista(string filtro = "")
        {
            lvClientes.Items.Clear();
            var lista = string.IsNullOrWhiteSpace(filtro)
                ? _gestor.ObtenerTodos().ToList()
                : _gestor.BuscarPorNombre(filtro);

            foreach (var c in lista)
            {
                var item = new ListViewItem(c.Id.ToString());
                item.SubItems.Add(c.Nombre);
                item.SubItems.Add(c.Telefono);
                item.SubItems.Add(c.TotalReservas.ToString());
                item.Tag = c.Id;
                lvClientes.Items.Add(item);
            }
        }

        private void BtnAgregar_Click(object? s, EventArgs e)
        {
            try
            {
                if (!Validador.EsNombreValido(txtNombre.Text))
                    throw new Exception("El nombre debe tener al menos 2 caracteres.");
                if (!Validador.EsTelefonoValido(txtTelefono.Text))
                    throw new Exception("Teléfono inválido (7-15 dígitos).");
                if (!Validador.EsEmailValido(txtEmail.Text))
                    throw new Exception("Email inválido.");

                var cliente = new Cliente(txtNombre.Text, txtTelefono.Text, txtEmail.Text);
                _gestor.Agregar(cliente);
                MostrarMensaje("✅ Cliente registrado correctamente.", Color.Green);
                txtNombre.Clear(); txtTelefono.Clear(); txtEmail.Clear();
                CargarLista();
            }
            catch (Exception ex) { MostrarMensaje($"❌ {ex.Message}", Color.Red); }
        }

        private void BtnEliminar_Click(object? s, EventArgs e)
        {
            if (lvClientes.SelectedItems.Count == 0) { MostrarMensaje("⚠️ Seleccione un cliente.", Color.Orange); return; }
            int id = (int)lvClientes.SelectedItems[0].Tag!;
            var res = MessageBox.Show("¿Eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != DialogResult.Yes) return;
            try
            {
                _gestor.Eliminar(id);
                MostrarMensaje("✅ Cliente eliminado.", Color.Green);
                CargarLista();
            }
            catch (Exception ex) { MostrarMensaje($"❌ {ex.Message}", Color.Red); }
        }

        private void MostrarMensaje(string msg, Color color)
        {
            lblMensaje.Text      = msg;
            lblMensaje.ForeColor = color;
        }
    }
}
