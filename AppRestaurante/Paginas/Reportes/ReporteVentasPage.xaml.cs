using AppRestaurante.Modelos;
using AppRestaurante.Servicios;


namespace AppRestaurante.Paginas.Reportes
{
    public partial class ReporteVentasPage : ContentPage
    {
        private PedidoService _pedidoService;
        private List<PedidoReporte> _todosLosPedidos;
        private List<ReporteVenta> _ultimoReporte = new();


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

        private async void MostrarReporte(List<PedidoReporte> pedidos)
        {
            if (pedidos == null || pedidos.Count == 0)
            {
                await DisplayAlert("Advertencia", "No hay pedidos disponibles.", "OK");
                return;
            }

            foreach (var p in pedidos)
                System.Diagnostics.Debug.WriteLine($"Pedido: {p.Plato.Name} - Estado: '{p.Estado}' - Fecha: {p.Fecha}");

            var pagados = pedidos
                .Where(p => p.Estado == "pagado")
                .ToList();

            if (pagados.Count == 0)
            {
                await DisplayAlert("Advertencia", "Hay pedidos, pero ninguno tiene estado 'pagado'.", "OK");
            }

            var reporte = pagados
                .GroupBy(p => p.Plato.Name)
                .Select(g => new ReporteVenta
                {
                    Plato = g.Key,
                    CantidadVendida = g.Sum(p => p.Cantidad)
                })
                .OrderByDescending(r => r.CantidadVendida)
                .ToList();

            _ultimoReporte = reporte;
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
                    (!_inicioSeleccionado || p.Fecha.ToLocalTime().Date >= FechaInicioPicker.Date) &&
                    (!_finSeleccionado || p.Fecha.ToLocalTime().Date <= FechaFinPicker.Date))
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
        private async void VerGrafico_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (_ultimoReporte == null || !_ultimoReporte.Any())
                {
                    await DisplayAlert("Sin datos", "No hay datos para mostrar en el gráfico.", "OK");
                    return;
                }

                // Verifica datos antes de pasar
                foreach (var item in _ultimoReporte)
                {
                    if (item == null || string.IsNullOrWhiteSpace(item.Plato))
                    {
                        await DisplayAlert("Error de datos", "Hay elementos inválidos en el reporte.", "OK");
                        return;
                    }
                }

                await Navigation.PushAsync(new GraficoVentasPage(_ultimoReporte));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error inesperado", ex.Message, "OK");
            }
        }


    }
}
