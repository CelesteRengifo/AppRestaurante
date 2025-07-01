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
}