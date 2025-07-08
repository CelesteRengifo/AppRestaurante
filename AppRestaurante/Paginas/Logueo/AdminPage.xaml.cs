namespace AppRestaurante.Paginas.Logueo;

public partial class AdminPage : ContentPage
{
    public AdminPage()
    {
        InitializeComponent();
    }
    private async void IrAGestionarUsuarios(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Usuario.Editar());
    }

    private async void IrAGestionarRoles(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Usuario.GestionarRoles());
    }

    private async void IrARegistrarUsuario(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Usuario.Registrar());
    }

    private async void IrARegistrarRol(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Usuario.RegistrarRol());
    }

    private async void IrAAgregarInsumo(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Inventario.AgregarInsumos());
    }

    private async void IrAGestionarInsumo(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Inventario.GestionarInsumos());
    }

    private async void IrAReporteVentas(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Reportes.ReporteVentasPage());
    }

    private async void AgregarPlatos(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Usuario.AgregarPlatos());
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