using RestauranteReservas.Modelo;
using RestauranteReservas.Servicios;
using RestauranteReservas.Utilidades;

namespace RestauranteReservas.Vista
{
    public class FormMenu : Form
    {
        private readonly GestorMenu _gestor;

        private ComboBox  cmbCategoria  = null!;
        private TextBox   txtNombre     = null!;
        private TextBox   txtPrecio     = null!;
        private TextBox   txtDescripcion= null!;
        private TextBox   txtExtra      = null!;
        private Label     lblExtra      = null!;
        private ListView  lvPlatos      = null!;
        private Label     lblMensaje    = null!;

        public FormMenu(GestorMenu gestor)
        {
            _gestor = gestor;
            InicializarComponentes();
            CargarLista();
        }

        private void InicializarComponentes()
        {
            this.Text            = "🍜  Menú del Restaurante";
            this.Size            = new Size(860, 540);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Color.FromArgb(255, 240, 248);
            this.Font            = new Font("Segoe UI", 10);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(233, 30, 99) };
            pnlHeader.Controls.Add(new Label
            {
                Text = "🍜  Menú del Restaurante", ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(15, 12)
            });
            this.Controls.Add(pnlHeader);

            var pnlForm = new GroupBox
            {
                Text = "Agregar plato al menú", Location = new Point(15, 60),
                Size = new Size(400, 280), Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            int y = 25;
            void AddCtrl(string lbl, Control ctrl)
            {
                pnlForm.Controls.Add(new Label { Text = lbl, Location = new Point(10, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9) });
                ctrl.Location = new Point(120, y);
                pnlForm.Controls.Add(ctrl);
                y += 40;
            }

            cmbCategoria = new ComboBox { Size = new Size(260, 26), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategoria.Items.AddRange(new object[] { "Entrada", "Plato Principal", "Postre" });
            cmbCategoria.SelectedIndex = 0;
            cmbCategoria.SelectedIndexChanged += (s, e) => ActualizarCampoExtra();
            AddCtrl("Categoría:", cmbCategoria);

            txtNombre = new TextBox { Size = new Size(260, 26) };
            AddCtrl("Nombre:", txtNombre);

            txtPrecio = new TextBox { Size = new Size(260, 26) };
            AddCtrl("Precio ($):", txtPrecio);

            txtDescripcion = new TextBox { Size = new Size(260, 26) };
            AddCtrl("Descripción:", txtDescripcion);

            lblExtra = new Label { Text = "¿Vegetariano?", Location = new Point(10, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9) };
            txtExtra = new TextBox { Location = new Point(120, y), Size = new Size(260, 26), PlaceholderText = "sí / no  ó  tipo proteína" };
            pnlForm.Controls.Add(lblExtra);
            pnlForm.Controls.Add(txtExtra);
            y += 40;

            var btnAgregar = new Button
            {
                Text = "➕  Agregar Plato", Location = new Point(10, y + 5),
                Size = new Size(375, 32), BackColor = Color.FromArgb(233, 30, 99),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregar_Click;
            pnlForm.Controls.Add(btnAgregar);
            this.Controls.Add(pnlForm);

            var btnEliminar = new Button
            {
                Text = "🗑️  Eliminar seleccionado", Location = new Point(15, 350),
                Size = new Size(220, 32), BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            lblMensaje = new Label
            {
                Location = new Point(15, 392), Size = new Size(400, 24),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblMensaje);

            lvPlatos = new ListView
            {
                Location = new Point(430, 60), Size = new Size(400, 430),
                View = View.Details, FullRowSelect = true, GridLines = true, BackColor = Color.White
            };
            lvPlatos.Columns.Add("Nombre", 140);
            lvPlatos.Columns.Add("Categoría", 130);
            lvPlatos.Columns.Add("Precio", 80);
            this.Controls.Add(lvPlatos);

            ActualizarCampoExtra();
        }

        private void ActualizarCampoExtra()
        {
            lblExtra.Text = cmbCategoria.SelectedIndex switch
            {
                0 => "¿Vegetariano?",
                1 => "Tipo proteína:",
                2 => "¿Sin gluten?",
                _ => "Extra:"
            };
            txtExtra.PlaceholderText = cmbCategoria.SelectedIndex switch
            {
                0 => "sí / no",
                1 => "Res / Pollo / Cerdo / Pescado",
                2 => "sí / no",
                _ => ""
            };
        }

        private void CargarLista()
        {
            lvPlatos.Items.Clear();
            foreach (var p in _gestor.ObtenerTodos())
            {
                var item = new ListViewItem(p.Nombre);
                item.SubItems.Add(p.ObtenerCategoria());
                item.SubItems.Add(Formatos.FormatearMoneda(p.Precio));
                item.Tag = p.Nombre;
                item.BackColor = p switch
                {
                    PlatoEntrada    => Color.FromArgb(232, 255, 240),
                    PlatoPrincipal  => Color.FromArgb(232, 240, 255),
                    Postre          => Color.FromArgb(255, 240, 252),
                    _               => Color.White
                };
                lvPlatos.Items.Add(item);
            }
        }

        private void BtnAgregar_Click(object? s, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    throw new Exception("El nombre no puede estar vacío.");
                if (!Validador.EsPrecioValido(txtPrecio.Text, out decimal precio))
                    throw new Exception("El precio debe ser un número positivo.");
                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                    throw new Exception("La descripción no puede estar vacía.");

                Plato plato = cmbCategoria.SelectedIndex switch
                {
                    0 => new PlatoEntrada(txtNombre.Text, precio, txtDescripcion.Text,
                             txtExtra.Text.Trim().ToLower() is "sí" or "si" or "s"),
                    1 => new PlatoPrincipal(txtNombre.Text, precio, txtDescripcion.Text,
                             string.IsNullOrWhiteSpace(txtExtra.Text) ? "Sin especificar" : txtExtra.Text.Trim()),
                    2 => new Postre(txtNombre.Text, precio, txtDescripcion.Text,
                             txtExtra.Text.Trim().ToLower() is not ("sí" or "si" or "s")),
                    _ => throw new Exception("Categoría inválida.")
                };

                _gestor.Agregar(plato);
                MostrarMensaje("✅ Plato agregado al menú.", Color.Green);
                txtNombre.Clear(); txtPrecio.Clear(); txtDescripcion.Clear(); txtExtra.Clear();
                CargarLista();
            }
            catch (Exception ex) { MostrarMensaje($"❌ {ex.Message}", Color.Red); }
        }

        private void BtnEliminar_Click(object? s, EventArgs e)
        {
            if (lvPlatos.SelectedItems.Count == 0) { MostrarMensaje("⚠️ Seleccione un plato.", Color.Orange); return; }
            string nombre = (string)lvPlatos.SelectedItems[0].Tag!;
            if (MessageBox.Show($"¿Eliminar '{nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                _gestor.Eliminar(nombre);
                MostrarMensaje("✅ Plato eliminado.", Color.Green);
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
