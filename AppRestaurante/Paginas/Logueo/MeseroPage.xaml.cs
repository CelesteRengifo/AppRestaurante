namespace AppRestaurante.Paginas.Logueo;

public partial class MeseroPage : ContentPage
{
	public MeseroPage()
	{
		InitializeComponent();
	}
    private async void IrAMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Paginas.Usuario.Menu());
    }
}