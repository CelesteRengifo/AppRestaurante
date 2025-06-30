using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
namespace AppRestaurante.Paginas.Usuario;

public partial class RegistrarRol : ContentPage
{
    private readonly RolService _rolService;
    public RegistrarRol()
    {
        InitializeComponent();
        _rolService = new RolService();
    }
    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        string nombreRol = entryRol.Text?.Trim();

        if (string.IsNullOrWhiteSpace(nombreRol))
        {
            await DisplayAlert("Error", "Debe ingresar un nombre para el rol", "OK");
            return;
        }
        nombreRol = char.ToUpper(nombreRol[0]) + nombreRol.Substring(1).ToLower();

        var rolesExistentes = await _rolService.ObtenerRolesAsync();

        bool yaExiste = rolesExistentes.Any(r =>
            r.name.Trim().ToLower() == nombreRol.ToLower());

        if (yaExiste)
        {
            await DisplayAlert("Rol duplicado", "Ya existe un rol con ese nombre (aunque sea con diferente mayúscula).", "OK");
            limpiarCampos();
            return;
        }

        var nuevoRol = new Rol { name = nombreRol };
        string resultado = await _rolService.RegistrarRolAsync(nuevoRol);

        if (resultado == null)
        {
            await DisplayAlert("Éxito", "Rol registrado correctamente", "OK");
            limpiarCampos();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo registrar el rol", "OK");
            limpiarCampos();
        }
    }
    void limpiarCampos()
    {
        entryRol.Text = string.Empty;
        entryRol.Focus();
    }
}