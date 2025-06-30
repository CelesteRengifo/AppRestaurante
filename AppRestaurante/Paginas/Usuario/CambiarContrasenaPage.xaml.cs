using AppRestaurante.Servicios;
using Microsoft.Maui.Controls;
using System.Net.Http.Headers;
namespace AppRestaurante.Paginas.Usuario;

public partial class CambiarContrasenaPage : ContentPage
{
    private readonly UsuarioService _usuarioService;

    private bool verActual = false;
    private bool verNueva = false;
    private bool verConfirmar = false;

    public CambiarContrasenaPage()
    {
        InitializeComponent();
        _usuarioService = new UsuarioService();
    }

    private async void OnCambiarClicked(object sender, EventArgs e)
    {
        string actual = entryActual.Text?.Trim();
        string nueva = entryNueva.Text?.Trim();
        string confirmar = entryConfirmar.Text?.Trim();

        if (string.IsNullOrWhiteSpace(actual) || string.IsNullOrWhiteSpace(nueva) || string.IsNullOrWhiteSpace(confirmar))
        {
            await DisplayAlert("Error", "Completa todos los campos.", "OK");
            return;
        }

        if (nueva != confirmar)
        {
            await DisplayAlert("Error", "Las nuevas contraseñas no coinciden.", "OK");
            return;
        }

        bool ok = await _usuarioService.CambiarMiContrasenaAsync(actual, nueva);

        if (ok)
        {
            await DisplayAlert("Éxito", "Contraseña cambiada correctamente.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo cambiar la contraseña. Verifica tu contraseña actual.", "OK");
        }
    }

    private void OnVerActualClicked(object sender, EventArgs e)
    {
        verActual = !verActual;
        entryActual.IsPassword = !verActual;
        btnVerActual.Source = verActual ? "ocultar.png" : "ver.png";
    }

    private void OnVerNuevaClicked(object sender, EventArgs e)
    {
        verNueva = !verNueva;
        entryNueva.IsPassword = !verNueva;
        btnVerNueva.Source = verNueva ? "ocultar.png" : "ver.png";
    }

    private void OnVerConfirmarClicked(object sender, EventArgs e)
    {
        verConfirmar = !verConfirmar;
        entryConfirmar.IsPassword = !verConfirmar;
        btnVerConfirmar.Source = verConfirmar ? "ocultar.png" : "ver.png";
    }
}