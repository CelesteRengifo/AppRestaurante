using AppRestaurante.Paginas.Logueo;
using AppRestaurante.Paginas.Reportes;
using Syncfusion.Licensing;
namespace AppRestaurante
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JEaF5cXmRCeUx3QXxbf1x1ZFdMYVhbRnJPIiBoS35Rc0VkWXhfdHVRRGRYVUB3VEFd");
            MainPage = new AppShell();
            //MainPage = new AppRestaurante.Paginas.Usuario.InicioSesion();
            //MainPage = new NavigationPage(new AdminPage());
            //MainPage = new NavigationPage(new Paginas.Usuario.EditarUsuarioPage());
            //MainPage = new AppRestaurante.Paginas.Usuario.Registrar();
            //MainPage = new NavigationPage(new ReporteVentasPage());

        }
    }
}