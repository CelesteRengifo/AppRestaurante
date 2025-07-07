using AppRestaurante.Modelos;
using AppRestaurante.Servicios;

namespace AppRestaurante.Paginas.Reportes;

public partial class OpcionesExportarPage : ContentPage
{
    private readonly List<ReporteVenta> _reporte;
    private readonly DateTime? _fechaInicio;
    private readonly DateTime? _fechaFin;

    public OpcionesExportarPage(List<ReporteVenta> reporte, DateTime? inicio = null, DateTime? fin = null)
    {
        InitializeComponent();
        _reporte = reporte;
        _fechaInicio = inicio;
        _fechaFin = fin;
    }

    private async void Imprimir_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (_reporte == null || !_reporte.Any())
            {
                await DisplayAlert("Sin datos", "No hay datos para imprimir.", "OK");
                return;
            }

            var exportador = new ExportadorPDF();
            string ruta = await exportador.GenerarReportePDFAsync(
                _reporte,
                _fechaInicio,
                _fechaFin,
                FechaSwitch.IsToggled // ✅ solo 4 argumentos
            );

            await Launcher.Default.OpenAsync(new OpenFileRequest("Imprimir", new ReadOnlyFile(ruta)));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo generar el PDF: {ex.Message}", "OK");
        }
    }

    private async void Exportar_Clicked(object sender, EventArgs e)
    {
        string formato = ((Button)sender).Text.ToLower();
        bool incluirFecha = FechaSwitch.IsToggled;

        try
        {
            if (_reporte == null || !_reporte.Any())
            {
                await DisplayAlert("Sin datos", "No hay datos para exportar.", "OK");
                return;
            }

            string ruta = "";

            if (formato == "pdf")
            {
                var exportador = new ExportadorPDF();
                ruta = await exportador.GenerarReportePDFAsync(_reporte, _fechaInicio, _fechaFin, incluirFecha);
            }
            else if (formato == "excel")
            {
                var exportador = new ExportadorExcel();
                ruta = await exportador.ExportarAExcelAsync(_reporte, _fechaInicio, _fechaFin, incluirFecha);
            }
            else if (formato == "csv")
            {
                var exportador = new ExportadorExcel();
                ruta = await exportador.ExportarACsvAsync(_reporte, incluirFecha);
            }

            await Launcher.Default.OpenAsync(new OpenFileRequest("Archivo generado", new ReadOnlyFile(ruta)));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo exportar: {ex.Message}", "OK");
        }
    }
}
