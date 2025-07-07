using AppRestaurante.Paginas.Logueo;
using AppRestaurante.Paginas.Reportes;
namespace AppRestaurante
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new AppShell();
            //MainPage = new AppRestaurante.Paginas.Usuario.InicioSesion();
            //MainPage = new NavigationPage(new AdminPage());
            //MainPage = new NavigationPage(new Paginas.Usuario.EditarUsuarioPage());
            //MainPage = new AppRestaurante.Paginas.Usuario.Registrar();
            MainPage = new AppRestaurante.Paginas.Reportes.ReporteVentasPage();
        }
    }
}