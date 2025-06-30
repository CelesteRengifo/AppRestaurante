using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
namespace AppRestaurante.Paginas.Usuario;

public partial class Registrar : ContentPage
{
    private readonly AuthService _authService;
    private readonly RolService _rolService;
    private List<Rol> rolesDisponibles;

    public Registrar()
    {
        InitializeComponent();
        _authService = new AuthService();
        _rolService = new RolService();
        rolesDisponibles = new List<Rol>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var roles = await _rolService.ObtenerRolesAsync();
        rolesDisponibles = roles;

        pickerRoles.ItemsSource = rolesDisponibles;
    }

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        string usuario = entryUsuario.Text?.Trim();
        string clave = entryPassword.Text;
        string nombre = entryNombre.Text?.Trim();
        string apellido = entryApellido.Text?.Trim();
        string correo = entryEmail.Text?.Trim();
        var rolSeleccionado = pickerRoles.SelectedItem as Rol;

        if (string.IsNullOrWhiteSpace(usuario) ||
        string.IsNullOrWhiteSpace(clave) ||
        string.IsNullOrWhiteSpace(nombre) ||
        string.IsNullOrWhiteSpace(apellido) ||
        string.IsNullOrWhiteSpace(correo) ||
        rolSeleccionado == null)
        {
            await DisplayAlert("Error", "Completa todos los campos.", "OK");
            LimpiarCampos();
            return;
        }

        // Limpiar y capitalizar
        nombre = char.ToUpper(nombre[0]) + nombre.Substring(1).ToLower();
        apellido = char.ToUpper(apellido[0]) + apellido.Substring(1).ToLower();

        var nuevoUsuario = new RegistroRequest
        {
            username = usuario,
            password = clave,
            first_name = nombre,
            last_name = apellido,
            email = correo,
            rol_id = rolSeleccionado.id
        };

        var resultado = await _authService.RegistrarUsuarioAsync(nuevoUsuario);

        if (resultado == null)
        {
            await DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
            LimpiarCampos();
        }
        else if (resultado.Contains("username") && resultado.Contains("already exists"))
        {
            await DisplayAlert("Error", "El nombre de usuario ya está en uso.", "OK");
            LimpiarCampos();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo registrar el usuario.", "OK");
            LimpiarCampos();
        }
    }

    private void LimpiarCampos()
    {
        entryUsuario.Text = "";
        entryPassword.Text = "";
        entryNombre.Text = "";
        entryApellido.Text = "";
        pickerRoles.SelectedIndex = -1;
    }
}