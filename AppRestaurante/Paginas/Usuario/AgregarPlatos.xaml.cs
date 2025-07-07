using AppRestaurante.Servicios;
using Microsoft.Maui.Storage;

namespace AppRestaurante.Paginas.Usuario;

public partial class AgregarPlatos : ContentPage
{
    private readonly AuthService _authService = new();
    private FileResult _imagenSeleccionada;
    public AgregarPlatos()
	{
		InitializeComponent();
	}
    private async void SeleccionarImagen_Clicked(object sender, EventArgs e)
    {
        try
        {
            _imagenSeleccionada = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una imagen",
                FileTypes = FilePickerFileType.Images
            });

            if (_imagenSeleccionada != null)
            {
                var stream = await _imagenSeleccionada.OpenReadAsync();
                ImagenPreview.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo seleccionar imagen: {ex.Message}", "OK");
        }
    }

    private async void SubirPlato_Clicked(object sender, EventArgs e)
    {
        string nombre = NombreEntry.Text;
        string precioTexto = PrecioEntry.Text;

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(precioTexto) || _imagenSeleccionada == null)
        {
            await DisplayAlert("Advertencia", "Completa todos los campos y selecciona una imagen.", "OK");
            return;
        }

        if (!decimal.TryParse(precioTexto, out decimal precioDecimal))
        {
            await DisplayAlert("Error", "El precio no es válido.", "OK");
            return;
        }

        int precioEntero = (int)Math.Round(precioDecimal); // Convertir a entero

        try
        {
            var stream = await _imagenSeleccionada.OpenReadAsync();
            await _authService.SubirPlatoAsync(nombre, precioEntero, stream, _imagenSeleccionada.FileName);

            await DisplayAlert("Éxito", "Plato subido correctamente.", "OK");
            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
