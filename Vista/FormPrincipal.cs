using RestauranteReservas.Servicios;
using RestauranteReservas.Utilidades;

namespace RestauranteReservas.Vista
{
    public partial class FormPrincipal : Form
    {
        // Servicios compartidos entre todos los formularios
        private readonly GestorClientes  _gestorClientes  = new();
        private readonly GestorMesas     _gestorMesas     = new();
        private readonly GestorReservas  _gestorReservas  = new();
        private readonly GestorMenu      _gestorMenu      = new();

        private Label lblTotalReservas = null!;
        private Label lblTotalClientes = null!;
        private Label lblMesasDisp     = null!;
        private Label lblTotalPlatos   = null!;

        public FormPrincipal()
        {
            InitializeComponent();
            DatosIniciales.Cargar(_gestorMesas, _gestorClientes, _gestorMenu, _gestorReservas);
            ActualizarResumen();
        }

        private void InitializeComponent()
        {
            this.Text            = "🍽️  Restaurante Reservas";
            this.Size            = new Size(900, 620);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Color.FromArgb(255, 248, 240);
            this.Font            = new Font("Segoe UI", 10);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;

            // ── Header ────────────────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 80,
                BackColor = Color.FromArgb(62, 100, 80)
            };
            var lblTitulo = new Label
            {
                Text      = "🍽️  Sistema de Reservas – Restaurante",
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize  = true,
                Location  = new Point(20, 22)
            };
            pnlHeader.Controls.Add(lblTitulo);
            this.Controls.Add(pnlHeader);

            // ── Resumen (4 tarjetas) ──────────────────────────────────────────
            var pnlResumen = new Panel
            {
                Location  = new Point(20, 100),
                Size      = new Size(840, 100),
                BackColor = Color.Transparent
            };

            string[] titles = { "Reservas activas", "Clientes registrados", "Mesas disponibles", "Platos en menú" };
            Color[] colors  = {
                Color.FromArgb(255, 183, 77),
                Color.FromArgb(100, 181, 246),
                Color.FromArgb(129, 199, 132),
                Color.FromArgb(240, 98, 146)
            };
            var statLabels = new Label[4];

            for (int i = 0; i < 4; i++)
            {
                int xi  = i;
                var card = new Panel
                {
                    Location  = new Point(i * 210, 0),
                    Size      = new Size(195, 90),
                    BackColor = colors[i],
                    Cursor    = Cursors.Default
                };
                card.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                };

                var lblTitle = new Label
                {
                    Text      = titles[xi],
                    ForeColor = Color.White,
                    Font      = new Font("Segoe UI", 9),
                    AutoSize  = false,
                    Size      = new Size(185, 20),
                    Location  = new Point(5, 8),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                var lblNum = new Label
                {
                    Text      = "0",
                    ForeColor = Color.White,
                    Font      = new Font("Segoe UI", 28, FontStyle.Bold),
                    AutoSize  = false,
                    Size      = new Size(185, 50),
                    Location  = new Point(5, 32),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                statLabels[xi] = lblNum;
                card.Controls.Add(lblTitle);
                card.Controls.Add(lblNum);
                pnlResumen.Controls.Add(card);
            }

            lblTotalReservas = statLabels[0];
            lblTotalClientes = statLabels[1];
            lblMesasDisp     = statLabels[2];
            lblTotalPlatos   = statLabels[3];

            this.Controls.Add(pnlResumen);

            // ── Botones de navegación ─────────────────────────────────────────
            var pnlBotones = new Panel
            {
                Location  = new Point(20, 220),
                Size      = new Size(840, 310),
                BackColor = Color.Transparent
            };

            var btnData = new (string texto, string icono, Color color, Action accion)[]
            {
                ("Gestionar\nReservas",  "📅", Color.FromArgb(255, 152, 0),  AbrirReservas),
                ("Gestionar\nClientes",  "👥", Color.FromArgb(33, 150, 243),  AbrirClientes),
                ("Gestionar\nMesas",     "🪑", Color.FromArgb(76, 175, 80),   AbrirMesas),
                ("Menú del\nRestaurante","🍜", Color.FromArgb(233, 30, 99),   AbrirMenu),
            };

            for (int i = 0; i < btnData.Length; i++)
            {
                var (texto, icono, color, accion) = btnData[i];
                int col = i % 2, row = i / 2;

                var btn = new Button
                {
                    Text      = $"{icono}\n{texto}",
                    Size      = new Size(390, 130),
                    Location  = new Point(col * 420, row * 150),
                    BackColor = color,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI", 13, FontStyle.Bold),
                    Cursor    = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) => accion();

                // Hover effect
                btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Dark(color, 0.1f);
                btn.MouseLeave += (s, e) => btn.BackColor = color;

                pnlBotones.Controls.Add(btn);
            }

            this.Controls.Add(pnlBotones);

            // ── Footer ────────────────────────────────────────────────────────
            var lblFooter = new Label
            {
                Text      = "Herramientas de Programación I  ·  Proyecto Final  ·  2025",
                ForeColor = Color.Gray,
                Font      = new Font("Segoe UI", 8),
                AutoSize  = false,
                Size      = new Size(860, 20),
                Location  = new Point(20, 555),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblFooter);
        }

        private void ActualizarResumen()
        {
            lblTotalReservas.Text = _gestorReservas.ReservasActivas().ToString();
            lblTotalClientes.Text = _gestorClientes.TotalClientes().ToString();
            lblMesasDisp.Text     = _gestorMesas.MesasDisponibles().ToString();
            lblTotalPlatos.Text   = _gestorMenu.TotalPlatos().ToString();
        }

        private void AbrirReservas()
        {
            using var f = new FormReservas(_gestorReservas, _gestorClientes, _gestorMesas);
            f.ShowDialog();
            ActualizarResumen();
        }

        private void AbrirClientes()
        {
            using var f = new FormClientes(_gestorClientes);
            f.ShowDialog();
            ActualizarResumen();
        }

        private void AbrirMesas()
        {
            using var f = new FormMesas(_gestorMesas);
            f.ShowDialog();
            ActualizarResumen();
        }

        private void AbrirMenu()
        {
            using var f = new FormMenu(_gestorMenu);
            f.ShowDialog();
            ActualizarResumen();
        }
    }
}
