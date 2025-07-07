using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
namespace AppRestaurante.Paginas.Reportes;

public partial class ReporteVentasPage : ContentPage
{
    private PedidoService _pedidoService;

    public ReporteVentasPage()
    {
        InitializeComponent();
        _pedidoService = new PedidoService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarReporte(); // Aquí es seguro usar ReporteCollectionView
    }

    private async Task CargarReporte()
    {
        try
        {
            var pedidos = await _pedidoService.ObtenerPedidosAsync();

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
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo generar el reporte: {ex.Message}", "OK");
        }
    }
}

public class ReporteVenta
{
    public string Plato { get; set; }
    public int CantidadVendida { get; set; }
}