namespace AppRestaurante.Paginas.Logueo;

public partial class MeseroPage : ContentPage
{
    private string token;

    public MeseroPage(string firstName, string lastName, string jwtToken)
    {
        InitializeComponent(); // Esto funcionará bien si el XAML no tiene errores
        token = jwtToken;

        string nombreCompleto = $"{firstName} {lastName}";
        lblBienvenida.Text = $"¡Bienvenido(a), {nombreCompleto}!";

        // ?? Esto oculta el botón de retroceso
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });
    }

    private async void IrAMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Usuario.Menu());
    }

    private async void CerrarSesion(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Cerrar sesión", "¿Deseas salir de la sesión?", "Sí", "No");
        if (confirm)
        {
            Preferences.Remove("accessToken");
            Preferences.Remove("usuario");
            Preferences.Remove("rol");

            // Volver a la página de inicio
            await Shell.Current.GoToAsync("//InicioSesion");
        }
    }
}
