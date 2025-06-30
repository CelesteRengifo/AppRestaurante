using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
using System.Globalization;

namespace AppRestaurante.Paginas.Inventario
{
    public partial class EditarInsumo : ContentPage
    {
        private readonly Insumo insumoOriginal;

        public EditarInsumo(Insumo insumo)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            insumoOriginal = insumo;

            // Precargar campos con valores actuales
            entryNombre.Text = insumo.name;
            entryStock.Text = insumo.stock.ToString(CultureInfo.InvariantCulture);
            entryPrecio.Text = "";
            pickerUnidad.SelectedItem = insumo.unidadMedida;
        }

        private async void btnActualizar_Clicked(object sender, EventArgs e)
        {
            string unidad = pickerUnidad.SelectedItem?.ToString();
            string precioTexto = entryPrecio.Text?.Trim();
            string stockTexto = entryStock.Text?.Trim();

            if (string.IsNullOrWhiteSpace(unidad) || string.IsNullOrWhiteSpace(stockTexto))
            {
                await DisplayAlert("Error", "Por favor complete todos los campos obligatorios.", "OK");
                return;
            }

            stockTexto = stockTexto.Replace(',', '.');
            precioTexto = precioTexto.Replace(',', '.');

            if (!decimal.TryParse(stockTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal stock))
            {
                await DisplayAlert("Error", "Stock inválido.", "OK");
                return;
            }

            decimal precioSumado = insumoOriginal.precio;

            if (!string.IsNullOrWhiteSpace(precioTexto))
            {
                if (!decimal.TryParse(precioTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal nuevoPrecio))
                {
                    await DisplayAlert("Error", "Precio inválido.", "OK");
                    return;
                }

                precioSumado += nuevoPrecio;
            }

            var insumoActualizado = new Insumo
            {
                id = insumoOriginal.id,
                name = insumoOriginal.name, // se mantiene sin cambios
                stock = stock,
                unidadMedida = unidad,
                precio = precioSumado
            };

            var servicio = new InsumoService();
            bool exito = await servicio.EditarInsumo(insumoActualizado);

            if (exito)
            {
                await DisplayAlert("Éxito", "Insumo actualizado correctamente.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo actualizar el insumo.", "OK");
            }
        }

        private async void btnRegresar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
