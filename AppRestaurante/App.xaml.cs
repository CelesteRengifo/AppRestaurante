using AppRestaurante.Paginas.Inventario;
using AppRestaurante.Paginas.Logueo;
using AppRestaurante.Paginas.Usuario;
namespace AppRestaurante
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new InicioSesion();
            MainPage = new NavigationPage(new MeseroPage());
            //MainPage = new NavigationPage(new Paginas.Usuario.EditarUsuarioPage());
            //MainPage = new AppRestaurante.Paginas.Usuario.Registrar();
            //MainPage = new AppRestaurante.Paginas.Usuario.Menu();
        }
    }
}