using AppRestaurante.Modelos;
using System.Collections.ObjectModel;

namespace AppRestaurante.Paginas.Reportes
{
    public partial class GraficoVentasPage : ContentPage
    {
        public ObservableCollection<ReporteVenta> Ventas { get; set; }

        public double MaximoY { get; set; }
        private List<ReporteVenta> _reporteOriginal;
        private readonly DateTime? _fechaInicio;
        private readonly DateTime? _fechaFin;

        public GraficoVentasPage(List<ReporteVenta> reporte, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error de gráfico", ex.Message, "OK");
                return;
            }

            _reporteOriginal = reporte ?? new List<ReporteVenta>();
            _fechaInicio = fechaInicio;
            _fechaFin = fechaFin;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Ventas = new ObservableCollection<ReporteVenta>();

            foreach (var item in _reporteOriginal)
            {
                if (item != null && !string.IsNullOrWhiteSpace(item.Plato))
                {
                    Ventas.Add(item);
                }
            }

            //Calcular máximo del eje Y con espacio extra
            double valorMaximo = Ventas.Any() ? Ventas.Max(v => v.CantidadVendida) : 0;
            MaximoY = valorMaximo + 1;

            BindingContext = this;

            //Subtítulo dinámico
            if (_fechaInicio.HasValue && _fechaFin.HasValue)
            {
                if (_fechaInicio.Value.Date == _fechaFin.Value.Date)
                {
                    SubtituloFecha.Text = $"Reporte del {_fechaInicio.Value:dd/MM/yyyy}";
                }
                else
                {
                    SubtituloFecha.Text = $"Del {_fechaInicio.Value:dd/MM/yyyy} al {_fechaFin.Value:dd/MM/yyyy}";
                }
            }
            else
            {
                SubtituloFecha.Text = "Reporte general del año";
            }
        }

    }
}
