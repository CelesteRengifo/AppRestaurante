using AppRestaurante.Modelos;
using System.Collections.ObjectModel;

namespace AppRestaurante.Paginas.Reportes
{
    public partial class GraficoVentasPage : ContentPage
    {
        public ObservableCollection<ReporteVenta> Ventas { get; set; }

        private List<ReporteVenta> _reporteOriginal;

        public GraficoVentasPage(List<ReporteVenta> reporte)
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

            BindingContext = this;
        }
    }
}
