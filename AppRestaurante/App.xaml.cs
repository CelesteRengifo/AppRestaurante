using AppRestaurante.Paginas.Usuario;
namespace AppRestaurante
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppRestaurante.Paginas.Usuario.Menu();
        }
    }
}