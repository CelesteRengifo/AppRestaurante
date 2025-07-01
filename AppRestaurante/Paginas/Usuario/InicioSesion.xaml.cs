using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
namespace AppRestaurante.Paginas.Usuario;


public partial class InicioSesion : ContentPage
{
    private readonly AuthService _authService;
    private bool mostrarClave = false;

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
            username = entryUsuario.Text?.Trim(),
            password = entryClave.Text
        };

        // Validar campos
        if (string.IsNullOrWhiteSpace(datos.username) || string.IsNullOrWhiteSpace(datos.password))
        {
            // Puedes mostrar un mensaje de error si lo deseas
            LimpiarCampos();
            return;
        }

        // Llamar al servicio
        var loginInfo = await _authService.LoginAsync(datos);

        if (loginInfo != null)
        {
            // Guardar datos persistentes
            Preferences.Set("accessToken", loginInfo.access);
            Preferences.Set("usuario", loginInfo.username);
            Preferences.Set("rol", loginInfo.rol);

            // Redirigir directamente según el rol
            await RedirigirPorRol(loginInfo.rol);

            LimpiarCampos();
        }
        else
        {
            // Si deseas mantener un mensaje de error aquí, puedes dejarlo
            LimpiarCampos();
        }
    }

    private void btnVerClave_Clicked(object sender, EventArgs e)
    {
        mostrarClave = !mostrarClave;
        entryClave.IsPassword = !mostrarClave;

        btnVerClave.Source = mostrarClave ? "ocultar.png" : "ver.png";
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
        entryUsuario.Text = string.Empty;
        entryClave.Text = string.Empty;
        mostrarClave = false;
        entryClave.IsPassword = true;
        btnVerClave.Source = "ver.png";

    }
}