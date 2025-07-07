using AppRestaurante.Modelos;
using AppRestaurante.Servicios;

namespace AppRestaurante.Paginas.Reportes
{
    public partial class ReporteVentasPage : ContentPage
    {
        private PedidoService _pedidoService;
        private List<PedidoReporte> _todosLosPedidos;

        private bool _inicioSeleccionado = false;
        private bool _finSeleccionado = false;

        public ReporteVentasPage()
        {
            InitializeComponent();
            _pedidoService = new PedidoService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarPedidos();
            MostrarReporte(_todosLosPedidos);
            ActualizarPlaceholders();
        }

        private async Task CargarPedidos()
        {
            try
            {
                _todosLosPedidos = await _pedidoService.ObtenerPedidosAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar los pedidos: {ex.Message}", "OK");
            }
        }

        private void MostrarReporte(List<PedidoReporte> pedidos)
        {
            var reporte = pedidos
                .Where(p => p.Estado.ToLower() == "pagado")
                .GroupBy(p => p.Plato.Name)
                .Select(g => new ReporteVenta
                {
                    Plato = g.Key,
                    CantidadVendida = g.Sum(p => p.Cantidad)
                })
                .OrderByDescending(r => r.CantidadVendida)
                .ToList();

            ReporteCollectionView.ItemsSource = reporte;
        }

        private async void Filtrar_Clicked(object sender, EventArgs e)
        {
            if (_todosLosPedidos == null) return;

            if (!_inicioSeleccionado && !_finSeleccionado)
            {
                MostrarReporte(_todosLosPedidos);
                return;
            }

            if (_inicioSeleccionado && _finSeleccionado &&
                FechaFinPicker.Date < FechaInicioPicker.Date)
            {
                await DisplayAlert("Rango inválido", "La fecha 'Hasta' no puede ser menor que la fecha 'Desde'.", "OK");
                ResetearFechas();
                return;
            }

            var pedidosFiltrados = _todosLosPedidos
                .Where(p =>
                    (!_inicioSeleccionado || p.Fecha.Date >= FechaInicioPicker.Date) &&
                    (!_finSeleccionado || p.Fecha.Date <= FechaFinPicker.Date))
                .ToList();

            if (pedidosFiltrados.Count == 0)
            {
                await DisplayAlert("Sin resultados", "No se encontraron pedidos en el rango seleccionado.", "OK");
                ResetearFechas();
            }

            MostrarReporte(pedidosFiltrados);
        }

        private void Limpiar_Clicked(object sender, EventArgs e)
        {
            _inicioSeleccionado = false;
            _finSeleccionado = false;

            FechaInicioPicker.Date = DateTime.Today;
            FechaFinPicker.Date = DateTime.Today;

            PlaceholderDesde.Text = "Seleccionar...";
            PlaceholderHasta.Text = "Seleccionar...";

            MostrarReporte(_todosLosPedidos);
        }

        private void FechaInicioPicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            _inicioSeleccionado = true;
            PlaceholderDesde.Text = e.NewDate.ToString("dd/MM/yyyy");
        }

        private void FechaFinPicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            _finSeleccionado = true;
            PlaceholderHasta.Text = e.NewDate.ToString("dd/MM/yyyy");
        }

        private void PlaceholderDesde_Tapped(object sender, EventArgs e)
        {
            FechaInicioPicker.Focus();
        }

        private void PlaceholderHasta_Tapped(object sender, EventArgs e)
        {
            FechaFinPicker.Focus();
        }

        private void ActualizarPlaceholders()
        {
            PlaceholderDesde.Text = _inicioSeleccionado
                ? FechaInicioPicker.Date.ToString("dd/MM/yyyy")
                : "Seleccionar...";

            PlaceholderHasta.Text = _finSeleccionado
                ? FechaFinPicker.Date.ToString("dd/MM/yyyy")
                : "Seleccionar...";
        }
        private void ResetearFechas()
        {
            _inicioSeleccionado = false;
            _finSeleccionado = false;

            FechaInicioPicker.Date = DateTime.Today;
            FechaFinPicker.Date = DateTime.Today;

            PlaceholderDesde.Text = "Seleccionar...";
            PlaceholderHasta.Text = "Seleccionar...";
        }

    }

    public class ReporteVenta
    {
        public string Plato { get; set; }
        public int CantidadVendida { get; set; }
    }
}
