using AppRestaurante.Paginas.Inventario;
using AppRestaurante.Paginas.Usuario;
namespace AppRestaurante
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new CambiarContrasenaPage();
            //MainPage = new NavigationPage(new Paginas.Usuario.Editar());
            //MainPage = new AppRestaurante.Paginas.Usuario.Registrar();
            //MainPage = new AppRestaurante.Paginas.Usuario.Menu();
        }
    }
}