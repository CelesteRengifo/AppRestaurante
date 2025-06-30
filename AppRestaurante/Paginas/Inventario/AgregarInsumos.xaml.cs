using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
using System.Globalization;

namespace AppRestaurante.Paginas.Inventario
{
    public partial class AgregarInsumos : ContentPage
    {
        private List<Insumo> insumosExistentes = new();

        public AgregarInsumos()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            CargarInsumos();
        }

        private async void CargarInsumos()
        {
            var servicio = new InsumoService();
            insumosExistentes = await servicio.ObtenerInsumosAsync();
        }

        private async void btnAgregarInsumo_Clicked(object sender, EventArgs e)
        {
            string nombre = entryNombre.Text?.Trim();
            string unidad = pickerUnidad.SelectedItem?.ToString();
            string precioTexto = entryPrecio.Text?.Trim();
            string stockTexto = entryStock.Text?.Trim();

            // Limpiar espacios dobles y normalizar
            if (!string.IsNullOrEmpty(nombre))
            {
                nombre = System.Text.RegularExpressions.Regex.Replace(nombre, @"\s+", " ").Trim();
                if (nombre.Length > 0)
                    nombre = char.ToUpper(nombre[0]) + nombre.Substring(1).ToLower();
            }

            // Validación de campos vacíos
            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(unidad) ||
                string.IsNullOrWhiteSpace(precioTexto) ||
                string.IsNullOrWhiteSpace(stockTexto))
            {
                await DisplayAlert("Error", "Por favor complete todos los campos.", "OK");
                return;
            }

            // Reemplazo de coma por punto
            precioTexto = precioTexto.Replace(',', '.');
            stockTexto = stockTexto.Replace(',', '.');

            // Validación decimal con cultura invariable
            if (!decimal.TryParse(precioTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
            {
                await DisplayAlert("Error", "Precio debe ser un número válido.", "OK");
                return;
            }

            if (!decimal.TryParse(stockTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal stock))
            {
                await DisplayAlert("Error", "Stock debe ser un número válido.", "OK");
                return;
            }

            // Verificación de duplicado
            if (insumosExistentes != null && insumosExistentes.Any(i => i.name.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Ya existe", $"El insumo '{nombre}' ya fue registrado.", "OK");
                return;
            }

            var insumo = new Insumo
            {
                name = nombre,
                stock = stock,
                precio = precio,
                unidadMedida = unidad
            };

            var servicio = new InsumoService();
            var exito = await servicio.AgregarInsumoAsync(insumo);

            if (exito)
            {
                await DisplayAlert("Éxito", $"Insumo '{nombre}' guardado correctamente.", "OK");

                // Limpiar campos
                entryNombre.Text = "";
                entryPrecio.Text = "";
                entryStock.Text = "";
                pickerUnidad.SelectedIndex = -1;

                // Agregar a lista local
                insumosExistentes.Add(insumo);
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar el insumo.", "OK");
            }
        }
        private async void btnRegresar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Vuelve a la pantalla anterior (GestionarInsumos)
        }

    }
}
