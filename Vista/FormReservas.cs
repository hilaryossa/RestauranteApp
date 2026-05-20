using RestauranteReservas.Modelo;
using RestauranteReservas.Servicios;
using RestauranteReservas.Utilidades;

namespace RestauranteReservas.Vista
{
    public class FormReservas : Form
    {
        private readonly GestorReservas _gestorReservas;
        private readonly GestorClientes _gestorClientes;
        private readonly GestorMesas    _gestorMesas;

        private ComboBox      cmbCliente      = null!;
        private ComboBox      cmbMesa         = null!;
        private DateTimePicker dtpFecha        = null!;
        private DateTimePicker dtpHora         = null!;
        private TextBox       txtPersonas     = null!;
        private TextBox       txtObservaciones= null!;
        private ListView      lvReservas      = null!;
        private Label         lblMensaje      = null!;

        public FormReservas(GestorReservas gestorReservas, GestorClientes gestorClientes, GestorMesas gestorMesas)
        {
            _gestorReservas = gestorReservas;
            _gestorClientes = gestorClientes;
            _gestorMesas    = gestorMesas;
            InicializarComponentes();
            CargarCombos();
            CargarLista();
        }

        private void InicializarComponentes()
        {
            this.Text            = "📅  Gestión de Reservas";
            this.Size            = new Size(960, 600);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Color.FromArgb(255, 250, 240);
            this.Font            = new Font("Segoe UI", 10);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(255, 152, 0) };
            pnlHeader.Controls.Add(new Label
            {
                Text = "📅  Reservas del Restaurante", ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(15, 12)
            });
            this.Controls.Add(pnlHeader);

            // Formulario izquierdo
            var pnlForm = new GroupBox
            {
                Text = "Nueva reserva", Location = new Point(15, 60),
                Size = new Size(420, 350), Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            int y = 25;
            void AddRow(string lbl, Control ctrl)
            {
                pnlForm.Controls.Add(new Label { Text = lbl, Location = new Point(10, y + 3), AutoSize = true, Font = new Font("Segoe UI", 9) });
                ctrl.Location = new Point(130, y);
                pnlForm.Controls.Add(ctrl);
                y += 42;
            }

            cmbCliente = new ComboBox { Size = new Size(270, 26), DropDownStyle = ComboBoxStyle.DropDownList };
            AddRow("Cliente:", cmbCliente);

            cmbMesa = new ComboBox { Size = new Size(270, 26), DropDownStyle = ComboBoxStyle.DropDownList };
            AddRow("Mesa disponible:", cmbMesa);

            dtpFecha = new DateTimePicker { Size = new Size(270, 26), Format = DateTimePickerFormat.Short, MinDate = DateTime.Today };
            AddRow("Fecha:", dtpFecha);

            dtpHora = new DateTimePicker { Size = new Size(270, 26), Format = DateTimePickerFormat.Time, ShowUpDown = true };
            AddRow("Hora:", dtpHora);

            txtPersonas = new TextBox { Size = new Size(270, 26) };
            AddRow("Personas:", txtPersonas);

            txtObservaciones = new TextBox { Size = new Size(270, 52), Multiline = true };
            AddRow("Observaciones:", txtObservaciones);
            y += 10;

            var btnCrear = new Button
            {
                Text = "📅  Crear Reserva", Location = new Point(10, y),
                Size = new Size(195, 35), BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCrear.FlatAppearance.BorderSize = 0;
            btnCrear.Click += BtnCrear_Click;
            pnlForm.Controls.Add(btnCrear);

            this.Controls.Add(pnlForm);

            // Botones de estado
            var pnlAcciones = new GroupBox
            {
                Text = "Acciones sobre reserva seleccionada",
                Location = new Point(15, 418), Size = new Size(420, 120),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            var btnConfirmar = new Button
            {
                Text = "✅ Confirmar", Location = new Point(10, 25), Size = new Size(120, 32),
                BackColor = Color.FromArgb(76, 175, 80), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.Click += (s, e) => CambiarEstado(_gestorReservas.Confirmar, "confirmada");

            var btnCancelar = new Button
            {
                Text = "❌ Cancelar", Location = new Point(145, 25), Size = new Size(120, 32),
                BackColor = Color.FromArgb(244, 67, 54), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => CambiarEstado(_gestorReservas.Cancelar, "cancelada");

            var btnCompletar = new Button
            {
                Text = "🏁 Completar", Location = new Point(280, 25), Size = new Size(125, 32),
                BackColor = Color.FromArgb(103, 58, 183), ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnCompletar.FlatAppearance.BorderSize = 0;
            btnCompletar.Click += (s, e) => CambiarEstado(_gestorReservas.Completar, "completada");

            var btnEliminar = new Button
            {
                Text = "🗑️ Eliminar", Location = new Point(10, 68), Size = new Size(120, 32),
                BackColor = Color.Gray, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;

            pnlAcciones.Controls.AddRange(new Control[] { btnConfirmar, btnCancelar, btnCompletar, btnEliminar });
            this.Controls.Add(pnlAcciones);

            lblMensaje = new Label
            {
                Location = new Point(15, 545), Size = new Size(420, 22),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            this.Controls.Add(lblMensaje);

            // ListView reservas
            lvReservas = new ListView
            {
                Location = new Point(450, 60), Size = new Size(490, 480),
                View = View.Details, FullRowSelect = true, GridLines = true, BackColor = Color.White
            };
            lvReservas.Columns.Add("#", 35);
            lvReservas.Columns.Add("Cliente", 110);
            lvReservas.Columns.Add("Mesa", 45);
            lvReservas.Columns.Add("Fecha", 120);
            lvReservas.Columns.Add("Pers.", 45);
            lvReservas.Columns.Add("Estado", 90);
            this.Controls.Add(lvReservas);
        }

        private void CargarCombos()
        {
            cmbCliente.Items.Clear();
            foreach (var c in _gestorClientes.ObtenerTodos())
                cmbCliente.Items.Add(c);
            if (cmbCliente.Items.Count > 0) cmbCliente.SelectedIndex = 0;

            RefrescarComboMesas();
        }

        private void RefrescarComboMesas()
        {
            cmbMesa.Items.Clear();
            foreach (var m in _gestorMesas.ObtenerDisponibles())
                cmbMesa.Items.Add(m);
            if (cmbMesa.Items.Count > 0) cmbMesa.SelectedIndex = 0;
        }

        private void CargarLista()
        {
            lvReservas.Items.Clear();
            foreach (var r in _gestorReservas.ObtenerTodas())
            {
                var item = new ListViewItem(r.Id.ToString());
                item.SubItems.Add(r.Cliente.Nombre);
                item.SubItems.Add(r.Mesa.Numero.ToString());
                item.SubItems.Add(r.FechaHora.ToString("dd/MM HH:mm"));
                item.SubItems.Add(r.NumeroPersonas.ToString());
                item.SubItems.Add(r.Estado.ToString());
                item.Tag = r.Id;
                item.BackColor = r.Estado switch
                {
                    EstadoReserva.Pendiente   => Color.FromArgb(255, 255, 200),
                    EstadoReserva.Confirmada  => Color.FromArgb(220, 255, 220),
                    EstadoReserva.Cancelada   => Color.FromArgb(255, 220, 220),
                    EstadoReserva.Completada  => Color.FromArgb(220, 220, 255),
                    _                         => Color.White
                };
                lvReservas.Items.Add(item);
            }
        }

        private void BtnCrear_Click(object? s, EventArgs e)
        {
            try
            {
                if (cmbCliente.SelectedItem is not Cliente cliente)
                    throw new Exception("Seleccione un cliente.");
                if (cmbMesa.SelectedItem is not Mesa mesa)
                    throw new Exception("No hay mesas disponibles.");
                if (!Validador.EsEnteroPositivo(txtPersonas.Text, out int personas))
                    throw new Exception("Número de personas inválido.");

                var fechaHora = dtpFecha.Value.Date + dtpHora.Value.TimeOfDay;
                if (!Validador.EsFechaValida(fechaHora))
                    throw new Exception("La fecha/hora no puede ser en el pasado.");

                _gestorReservas.Crear(cliente, mesa, fechaHora, personas, txtObservaciones.Text);
                MostrarMensaje("✅ Reserva creada exitosamente.", Color.Green);
                txtPersonas.Clear(); txtObservaciones.Clear();
                RefrescarComboMesas();
                CargarLista();
            }
            catch (Exception ex) { MostrarMensaje($"❌ {ex.Message}", Color.Red); }
        }

        private void CambiarEstado(Action<int> accion, string texto)
        {
            if (lvReservas.SelectedItems.Count == 0) { MostrarMensaje("⚠️ Seleccione una reserva.", Color.Orange); return; }
            int id = (int)lvReservas.SelectedItems[0].Tag!;
            try
            {
                accion(id);
                MostrarMensaje($"✅ Reserva marcada como {texto}.", Color.Green);
                RefrescarComboMesas();
                CargarLista();
            }
            catch (Exception ex) { MostrarMensaje($"❌ {ex.Message}", Color.Red); }
        }

        private void BtnEliminar_Click(object? s, EventArgs e)
        {
            if (lvReservas.SelectedItems.Count == 0) { MostrarMensaje("⚠️ Seleccione una reserva.", Color.Orange); return; }
            int id = (int)lvReservas.SelectedItems[0].Tag!;
            if (MessageBox.Show("¿Eliminar esta reserva?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                _gestorReservas.Eliminar(id);
                MostrarMensaje("✅ Reserva eliminada.", Color.Green);
                RefrescarComboMesas();
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
