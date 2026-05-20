using RestauranteReservas.Modelo;
using RestauranteReservas.Servicios;
using RestauranteReservas.Utilidades;

namespace RestauranteReservas.Vista
{
    public class FormMesas : Form
    {
        private readonly GestorMesas _gestor;

        private TextBox   txtNumero    = null!;
        private TextBox   txtCapacidad = null!;
        private ComboBox  cmbUbicacion = null!;
        private ListView  lvMesas      = null!;
        private Label     lblMensaje   = null!;

        public FormMesas(GestorMesas gestor)
        {
            _gestor = gestor;
            InicializarComponentes();
            CargarLista();
        }

        private void InicializarComponentes()
        {
            this.Text            = "🪑  Gestión de Mesas";
            this.Size            = new Size(820, 500);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Color.FromArgb(240, 255, 244);
            this.Font            = new Font("Segoe UI", 10);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(76, 175, 80) };
            pnlHeader.Controls.Add(new Label
            {
                Text = "🪑  Mesas del Restaurante", ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(15, 12)
            });
            this.Controls.Add(pnlHeader);

            var pnlForm = new GroupBox
            {
                Text = "Agregar nueva mesa", Location = new Point(15, 60),
                Size = new Size(380, 190), Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            pnlForm.Controls.Add(new Label { Text = "Número:", Location = new Point(10, 28), AutoSize = true });
            txtNumero = new TextBox { Location = new Point(110, 25), Size = new Size(250, 26) };
            pnlForm.Controls.Add(txtNumero);

            pnlForm.Controls.Add(new Label { Text = "Capacidad:", Location = new Point(10, 68), AutoSize = true });
            txtCapacidad = new TextBox { Location = new Point(110, 65), Size = new Size(250, 26) };
            pnlForm.Controls.Add(txtCapacidad);

            pnlForm.Controls.Add(new Label { Text = "Ubicación:", Location = new Point(10, 108), AutoSize = true });
            cmbUbicacion = new ComboBox
            {
                Location = new Point(110, 105), Size = new Size(250, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbUbicacion.Items.AddRange(new object[] { "Interior", "Terraza", "Privado" });
            cmbUbicacion.SelectedIndex = 0;
            pnlForm.Controls.Add(cmbUbicacion);

            var btnAgregar = new Button
            {
                Text = "➕  Agregar Mesa", Location = new Point(10, 145),
                Size = new Size(355, 32), BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregar_Click;
            pnlForm.Controls.Add(btnAgregar);
            this.Controls.Add(pnlForm);

            var btnEliminar = new Button
            {
                Text = "🗑️  Eliminar seleccionada", Location = new Point(15, 260),
                Size = new Size(220, 32), BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            lblMensaje = new Label
            {
                Location = new Point(15, 300), Size = new Size(380, 24),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblMensaje);

            lvMesas = new ListView
            {
                Location = new Point(410, 60), Size = new Size(375, 390),
                View = View.Details, FullRowSelect = true, GridLines = true, BackColor = Color.White
            };
            lvMesas.Columns.Add("#", 45);
            lvMesas.Columns.Add("Capacidad", 80);
            lvMesas.Columns.Add("Ubicación", 90);
            lvMesas.Columns.Add("Estado", 110);
            this.Controls.Add(lvMesas);
        }

        private void CargarLista()
        {
            lvMesas.Items.Clear();
            foreach (var m in _gestor.ObtenerTodas())
            {
                var item = new ListViewItem(m.Numero.ToString());
                item.SubItems.Add(m.Capacidad.ToString());
                item.SubItems.Add(m.Ubicacion);
                item.SubItems.Add(m.Estado.ToString());
                item.Tag = m.Numero;
                item.BackColor = m.Estado switch
                {
                    EstadoMesa.Disponible => Color.FromArgb(232, 255, 232),
                    EstadoMesa.Reservada  => Color.FromArgb(255, 255, 200),
                    EstadoMesa.Ocupada    => Color.FromArgb(255, 230, 230),
                    _                     => Color.White
                };
                lvMesas.Items.Add(item);
            }
        }

        private void BtnAgregar_Click(object? s, EventArgs e)
        {
            try
            {
                if (!Validador.EsEnteroPositivo(txtNumero.Text, out int numero))
                    throw new Exception("El número de mesa debe ser un entero positivo.");
                if (!Validador.EsEnteroPositivo(txtCapacidad.Text, out int cap) || !Validador.EsCapacidadValida(cap))
                    throw new Exception("La capacidad debe estar entre 1 y 20.");

                _gestor.Agregar(new Mesa(numero, cap, cmbUbicacion.SelectedItem!.ToString()!));
                MostrarMensaje("✅ Mesa agregada.", Color.Green);
                txtNumero.Clear(); txtCapacidad.Clear();
                CargarLista();
            }
            catch (Exception ex) { MostrarMensaje($"❌ {ex.Message}", Color.Red); }
        }

        private void BtnEliminar_Click(object? s, EventArgs e)
        {
            if (lvMesas.SelectedItems.Count == 0) { MostrarMensaje("⚠️ Seleccione una mesa.", Color.Orange); return; }
            int numero = (int)lvMesas.SelectedItems[0].Tag!;
            if (MessageBox.Show("¿Eliminar esta mesa?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                _gestor.Eliminar(numero);
                MostrarMensaje("✅ Mesa eliminada.", Color.Green);
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
