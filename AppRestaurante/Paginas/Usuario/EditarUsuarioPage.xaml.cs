using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
using UsuarioModel = AppRestaurante.Modelos.Usuario;
namespace AppRestaurante.Paginas.Usuario;

public partial class EditarUsuarioPage : ContentPage
{
    private readonly UsuarioService _usuarioService;
    private readonly RolService _rolService;
    private UsuarioModel usuarioActual;
    public EditarUsuarioPage(UsuarioModel usuario)
    {
        InitializeComponent();
        _usuarioService = new UsuarioService();
        _rolService = new RolService();
        usuarioActual = usuario;
        CargarDatos();
    }

    private bool mostrarClave = false;

    private async void CargarDatos()
    {
        entryUsername.Text = usuarioActual.username;
        entryNombre.Text = usuarioActual.first_name;
        entryApellido.Text = usuarioActual.last_name;
        entryEmail.Text = usuarioActual.email;

        var roles = await _rolService.ObtenerRolesAsync();
        pickerRol.ItemsSource = roles;
        pickerRol.ItemDisplayBinding = new Binding("name");

        var rolSeleccionado = roles.FirstOrDefault(r => r.id == usuarioActual.rol_id);
        pickerRol.SelectedItem = rolSeleccionado;
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        string username = entryUsername.Text?.Trim();
        string nombre = entryNombre.Text?.Trim();
        string apellido = entryApellido.Text?.Trim();
        string correo = entryEmail.Text?.Trim();
        var rol = pickerRol.SelectedItem as Rol;

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(apellido) ||
            string.IsNullOrWhiteSpace(correo) ||
            rol == null)
        {
            await DisplayAlert("Error", "Completa todos los campos obligatorios.", "OK");
            return;
        }

        usuarioActual.username = username;
        usuarioActual.first_name = nombre;
        usuarioActual.last_name = apellido;
        usuarioActual.email = correo;
        usuarioActual.rol_id = rol.id;

        bool actualizado = await _usuarioService.ActualizarUsuarioAsync(usuarioActual);

        if (actualizado)
        {
            await DisplayAlert("Éxito", "Usuario actualizado correctamente.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo actualizar el usuario.", "OK");
        }
    }

}