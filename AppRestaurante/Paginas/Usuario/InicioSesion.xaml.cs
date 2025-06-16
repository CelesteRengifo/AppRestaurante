using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
namespace AppRestaurante.Paginas.Usuario;


public partial class InicioSesion : ContentPage
{
    private readonly AuthService _authService;
    public InicioSesion()
	{
		InitializeComponent();
        _authService = new AuthService();
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Animación del botón
        await btnLogin.ScaleTo(0.95, 100);
        await btnLogin.ScaleTo(1, 100);

        // Crear objeto con los datos del formulario
        var datos = new LoginRequest
        {
            Username = entryUsuario.Text?.Trim(),
            Password = entryClave.Text
        };

        // Validar campos
        if (string.IsNullOrWhiteSpace(datos.Username) || string.IsNullOrWhiteSpace(datos.Password))
        {
            await DisplayAlert("Error", "Debe ingresar usuario y contraseña", "OK");
            LimpiarCampos();
            return;
        }

        // Llamar al servicio
        var loginInfo = await _authService.LoginAsync(datos);

        if (loginInfo != null)
        {
            // Mostrar mensaje de bienvenida
            await DisplayAlert("Bienvenido", $"Hola {loginInfo.first_name}", "OK");

            // Guardar datos persistentes
            Preferences.Set("accessToken", loginInfo.access);
            Preferences.Set("usuario", loginInfo.username);
            Preferences.Set("rol", loginInfo.rol);

            // Mostrar el rol detectado (hasta que tengas las páginas creadas)
            await DisplayAlert("Rol detectado", $"Rol: {loginInfo.rol}", "OK");
            LimpiarCampos();
        }
        else
        {
            await DisplayAlert("Error", "Credenciales incorrectas", "OK");
            LimpiarCampos();
        }
    }

    private async Task RedirigirPorRol(string rol)
    {
        switch (rol.ToLower())
        {
            case "administrador":
                await Shell.Current.GoToAsync("//AdministradorPage");
                break;
            case "mesero":
                await Shell.Current.GoToAsync("//MeseroPage");
                break;
            case "cocinero":
                await Shell.Current.GoToAsync("//CocineroPage");
                break;
            default:
                await DisplayAlert("Error", $"Rol desconocido: {rol}", "OK");
                break;
        }
    }

    void LimpiarCampos()
    {
        entryUsuario.Text = " ";
        entryClave.Text = " ";

    }
}