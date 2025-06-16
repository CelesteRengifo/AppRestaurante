using AppRestaurante.Modelos;
using AppRestaurante.Servicios;
using System.Collections.ObjectModel;
namespace AppRestaurante.Paginas.Usuario;

public partial class GestionarRoles : ContentPage
{
    private RolService _rolService;
    private ObservableCollection<Rol> RolesOriginales;
    public ObservableCollection<Rol> RolesFiltrados { get; set; }

    public GestionarRoles()
    {
        InitializeComponent();
        _rolService = new RolService();
        RolesOriginales = new ObservableCollection<Rol>();
        RolesFiltrados = new ObservableCollection<Rol>();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarRoles();
    }

    private async Task CargarRoles()
    {
        var lista = await _rolService.ObtenerRolesAsync();

        RolesOriginales.Clear();
        foreach (var rol in lista)
            RolesOriginales.Add(rol);

        FiltrarRoles(searchBar.Text);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        FiltrarRoles(e.NewTextValue);
    }

    private void FiltrarRoles(string texto)
    {
        RolesFiltrados.Clear();

        var filtrados = string.IsNullOrWhiteSpace(texto)
            ? RolesOriginales
            : new ObservableCollection<Rol>(
                RolesOriginales.Where(r => r.name.ToLower().Contains(texto.ToLower())));

        foreach (var rol in filtrados)
            RolesFiltrados.Add(rol);
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var rol = (Rol)button.CommandParameter;

        bool confirmar = await DisplayAlert("Confirmar", $"¿Eliminar el rol '{rol.name}'?", "Sí", "No");

        if (confirmar)
        {
            bool eliminado = await _rolService.EliminarRolAsync(rol.id);
            if (eliminado)
            {
                await DisplayAlert("Éxito", "Rol eliminado", "OK");
                await CargarRoles();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar", "OK");
            }
        }
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var rol = (Rol)button.CommandParameter;

        string nuevoNombre = await DisplayPromptAsync("Editar Rol", "Nuevo nombre:", initialValue: rol.name);

        if (string.IsNullOrWhiteSpace(nuevoNombre))
            return;

        // Normaliza nombre: quita espacios y capitaliza
        nuevoNombre = nuevoNombre.Trim();
        nuevoNombre = char.ToUpper(nuevoNombre[0]) + nuevoNombre.Substring(1).ToLower();

        // Validar si es igual al actual
        if (nuevoNombre == rol.name)
        {
            await DisplayAlert("Sin cambios", "No realizaste ningún cambio.", "OK");
            return;
        }

        rol.name = nuevoNombre;

        bool actualizado = await _rolService.ActualizarRolAsync(rol);

        if (actualizado)
        {
            await DisplayAlert("Éxito", "Rol actualizado", "OK");
            await CargarRoles();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo actualizar", "OK");
        }
    }
}