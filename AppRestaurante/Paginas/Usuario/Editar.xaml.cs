using AppRestaurante.Servicios;
using System.Collections.ObjectModel;
using UsuarioModel = AppRestaurante.Modelos.Usuario;
namespace AppRestaurante.Paginas.Usuario;

public partial class Editar : ContentPage
{
    private UsuarioService _usuarioService;
    private RolService _rolService;
    private ObservableCollection<UsuarioModel> UsuariosOriginales;
    public ObservableCollection<UsuarioModel> UsuariosFiltrados { get; set; }

    public Editar()
    {
        InitializeComponent();
        _usuarioService = new UsuarioService();
        _rolService = new RolService();
        UsuariosOriginales = new ObservableCollection<UsuarioModel>();
        UsuariosFiltrados = new ObservableCollection<UsuarioModel>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarUsuarios();
    }

    private async Task CargarUsuarios()
    {
        var lista = await _usuarioService.ObtenerUsuariosAsync();
        UsuariosOriginales.Clear();
        foreach (var u in lista)
            UsuariosOriginales.Add(u);

        FiltrarUsuarios(searchBar.Text);
    }

    private void OnBuscarUsuario(object sender, TextChangedEventArgs e)
    {
        FiltrarUsuarios(e.NewTextValue);
    }

    private void FiltrarUsuarios(string texto)
    {
        UsuariosFiltrados.Clear();

        var filtrados = string.IsNullOrWhiteSpace(texto)
            ? UsuariosOriginales
            : new ObservableCollection<UsuarioModel>(UsuariosOriginales.Where(u =>
                u.username.ToLower().Contains(texto.ToLower()) ||
                u.first_name.ToLower().Contains(texto.ToLower()) ||
                u.last_name.ToLower().Contains(texto.ToLower())));

        foreach (var usuario in filtrados)
            UsuariosFiltrados.Add(usuario);
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;
        var usuario = (UsuarioModel)boton.CommandParameter;

        bool confirmar = await DisplayAlert("Confirmar", $"¿Eliminar al usuario '{usuario.username}'?", "Sí", "No");
        if (confirmar)
        {
            bool eliminado = await _usuarioService.EliminarUsuarioAsync(usuario.id);
            if (eliminado)
            {
                await DisplayAlert("Éxito", "Usuario eliminado", "OK");
                await CargarUsuarios();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar", "OK");
            }
        }
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        var boton = (Button)sender;
        var usuario = (UsuarioModel)boton.CommandParameter;
        await Navigation.PushAsync(new EditarUsuarioPage(usuario));
    }
}